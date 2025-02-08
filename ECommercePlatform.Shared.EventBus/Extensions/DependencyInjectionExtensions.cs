using ECommercePlatform.Shared.EventBus.Abstractions;
using ECommercePlatform.Shared.EventBus.Configuration;
using ECommercePlatform.Shared.EventBus.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommercePlatform.Shared.EventBus;

public static class DependencyInjectionExtensions
{
    public static IEventBusBuilder AddMessageBus(this IHostApplicationBuilder builder)
    {
        // rabbitmq specific services
        builder.Services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
        builder.Services.AddSingleton<IRabbitMqConnectionKeeper, RabbitMqConnectionKeeper>();

        // telemetry configuration
        builder.Services.AddSingleton<RabbitMqEventBusTelemetryOptions>();
        
        // configuration
        builder.Services.Configure<EventBusConfiguration>(builder.Configuration.GetSection(nameof(EventBusConfiguration)));

        // telemetry
        builder.Services.AddOpenTelemetry()
            .WithTracing(t => t.AddSource(RabbitMqEventBusTelemetryOptions.ActivitySourceName));

        // event bus
        builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();
        builder.Services.AddHostedService<RabbitMqEventBus>();

        return new EventBusBuilder(builder.Services);
    }

    private class EventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services { get; } = services;
    }
}