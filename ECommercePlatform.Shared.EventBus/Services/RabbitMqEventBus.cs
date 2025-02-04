using ECommercePlatform.Shared.EventBus.Abstractions;
using ECommercePlatform.Shared.EventBus.Events;
using ECommercePlatform.Shared.EventBus.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OpenTelemetry.Context.Propagation;
using OpenTelemetry;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using ECommercePlatform.Shared.EventBus.Attributes;
using ECommercePlatform.Shared.ServiceDefaults.Configuration;

using Newtonsoft.Json;

using Polly;
using Polly.Retry;

using RabbitMQ.Client.Exceptions;

namespace ECommercePlatform.Shared.EventBus.Services;

public sealed class RabbitMqEventBus(
    ILogger<RabbitMqEventBus> logger,
    IOptions<ServiceConfiguration> serviceConfigurationOptions,
    IOptions<EventBusConfiguration> evenBusConfigurationOptions,
    IOptions<EventBusSubscriptionOptions> eventBusSubscriptionOptions,
    IServiceProvider serviceProvider,
    IRabbitMqConnectionKeeper connectionKeeper,
    RabbitMqEventBusTelemetryOptions telemetryOptions) : IEventBus,
    IHostedService,
    IDisposable
{
    private const string ExchangeName = "EventBus";

    private readonly ServiceConfiguration _serviceConfiguration = serviceConfigurationOptions.Value;
    private readonly EventBusSubscriptionOptions _eventBusSubscriptionOptions = eventBusSubscriptionOptions.Value;
    
    private readonly ResiliencePipeline _publishingPipeline =
        CreateEventPublishingPipeline(evenBusConfigurationOptions.Value.PublishingRetryCount);
    private readonly TextMapPropagator _propagator = telemetryOptions.Propagator;
    private readonly ActivitySource _activitySource = telemetryOptions.ActivitySource;

    private string ServiceName => _serviceConfiguration.Name;
    
    private IChannel? _channel;
    private IConnection? _connection;
    
    public async Task PublishAsync(IntegrationEvent @event)
    {
        var routingKey = @event.GetType().GetCustomAttribute<IntegrationEventKeyAttribute>()?.Key;
        if (routingKey is null)
        {
            logger.LogWarning("Cannot obtain routing key for event '{eventName}'", @event.GetType().Name);
            return;
        }
        
        logger.LogTrace("Publishing event '{routingKey}'", routingKey);

        var connection = await connectionKeeper.Connection;
        var channel = await connection.CreateChannelAsync();
        
        await channel.ExchangeDeclareAsync(exchange: ExchangeName, type: ExchangeType.Direct);

        var eventSerialized = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(eventSerialized);
        
        var activityName = $"Publishing '{routingKey}'";

        var basicProperties = new BasicProperties
        {
            Headers = new Dictionary<string, object?>(),
            Persistent = true,
        };

        await _publishingPipeline.Execute(async () =>
        {
            using var activity = _activitySource.StartActivity(activityName, ActivityKind.Client);
            var activityContext = activity?.Context ?? Activity.Current?.Context;

            if (activityContext is not null)
            {
                _propagator.Inject(new PropagationContext(activityContext.Value,
                        Baggage.Current),
                    basicProperties,
                    InjectIntoBasicProperties);
            }
            
            SetActivityTags(activity, routingKey, operation: "publish");
            
            logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

            try
            {
                await channel.BasicPublishAsync(exchange: ExchangeName,
                    routingKey: routingKey,
                    basicProperties: basicProperties,
                    body: body,
                    mandatory: false);
            }
            catch (Exception ex)
            {
                SetExceptionTags(activity, ex);
            }
            
            return;

            static void InjectIntoBasicProperties(BasicProperties basicProperties, string key, string value)
            {
                basicProperties.Headers![key] = value;
            }
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                logger.LogInformation("Starting RabbitMQ connection on service '{ServiceName}'", ServiceName);

                _connection = await connectionKeeper.Connection;
                if (_connection.IsOpen)
                {
                    return;
                }
                
                logger.LogTrace("Creating consumer channel");
                
                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                _channel.CallbackExceptionAsync += (sender, ea) =>
                {
                    logger.LogWarning(ea.Exception, "Error occured while calling callback");
                    return Task.CompletedTask;
                };
                
                await _channel.ExchangeDeclareAsync(exchange: ExchangeName,
                    type: ExchangeType.Direct,
                    cancellationToken: cancellationToken);

                await _channel.QueueDeclareAsync(queue: ServiceName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null, 
                    cancellationToken: cancellationToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += OnMessageReceivedAsync;

                await _channel.BasicConsumeAsync(queue: ServiceName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: cancellationToken);

                foreach (var eventTypeName in _eventBusSubscriptionOptions.Subscriptions.Keys)
                {
                    await _channel.QueueBindAsync(queue: ServiceName,
                        exchange: ExchangeName,
                        routingKey: eventTypeName, cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error establishing RabbitMQ connection on service '{ServiceName}'", ServiceName);
            }
        }, cancellationToken);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }
    
    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var context = _propagator.Extract(default, eventArgs.BasicProperties, ExtractFromBasicProperties);
        
        var activityName = $"Receiving '{eventArgs.RoutingKey}' on '{ServiceName}'";
        using var activity = _activitySource.StartActivity(activityName, ActivityKind.Consumer, context.ActivityContext);
        
        SetActivityTags(activity, eventArgs.RoutingKey, operation: "receive");

        var eventName = eventArgs.RoutingKey;
        var body = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            activity?.SetTag("body", body);
            await ProcessEvent(eventName, body);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error processing message on service '{ServiceName}': {body}", ServiceName, body);
            SetExceptionTags(activity, ex);
        }

        return;

        static IEnumerable<string> ExtractFromBasicProperties(IReadOnlyBasicProperties basicProperties, string key)
        {
            var headers = basicProperties.Headers;
            if (headers is null || !headers.TryGetValue(key, out var value))
            {
                return [];
            }

            if (value is not byte[] bytes)
            {
                return [];
            }
            
            return [Encoding.UTF8.GetString(bytes)];
        }
    }

    private async Task ProcessEvent(string eventName, string body)
    {
        logger.LogTrace("Processing event '{eventName}'", eventName);
        
        await using var scope = serviceProvider.CreateAsyncScope();
        if (!_eventBusSubscriptionOptions.Subscriptions.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Cannot obtain event type for event name '{eventName}'", eventName);
            return;
        }

        if (JsonConvert.DeserializeObject(body, eventType) is not IntegrationEvent integrationEvent)
        {
            logger.LogWarning("Cannot deserialize event body '{body}' to integration event '{eventName}'", body, eventName);
            return;
        }
        
        var integrationEventHandlers = scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType);

        foreach (var integrationEventHandler in integrationEventHandlers)
        {
            await integrationEventHandler.Handle(integrationEvent);
        }
    }

    private static void SetActivityTags(Activity? activity, string routingKey, string operation)
    {
        if (activity is null)
        {
            return;
        }
        
        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.destination_kind", "queue");
        activity.SetTag("messaging.operation", operation);
        activity.SetTag("messaging.destination.name", routingKey);
        activity.SetTag("messaging.rabbitmq.routing_key", routingKey);
    }
    
    private static void SetExceptionTags(Activity? activity, Exception ex)
    {
        if (activity is null)
        {
            return;
        }
        
        activity.AddTag("exception.message", ex.Message);
        activity.AddTag("exception.stacktrace", ex.ToString());
        activity.AddTag("exception.type", ex.GetType().FullName);
        activity.SetStatus(ActivityStatusCode.Error);
    }

    private static ResiliencePipeline CreateEventPublishingPipeline(int retryCount)
    {
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
            MaxRetryAttempts = retryCount,
            DelayGenerator = (context) => ValueTask.FromResult(GetDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();
        
        static TimeSpan? GetDelay(int attempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, attempt));
        }
    }
}