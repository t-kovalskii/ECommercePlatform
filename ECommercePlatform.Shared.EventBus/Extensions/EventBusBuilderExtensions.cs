using System.Reflection;
using ECommercePlatform.Shared.EventBus.Abstractions;
using ECommercePlatform.Shared.EventBus.Attributes;
using ECommercePlatform.Shared.EventBus.Configuration;
using ECommercePlatform.Shared.EventBus.Events;

using Microsoft.Extensions.DependencyInjection;

namespace ECommercePlatform.Shared.EventBus;

public static class EventBusBuilderExtensions
{
    public static IEventBusBuilder
        AddSubscription<TIntegrationEvent, TIntegrationEventHandler>(this IEventBusBuilder builder)
        where TIntegrationEvent : IntegrationEvent
        where TIntegrationEventHandler : class, IIntegrationEventHandler<TIntegrationEvent>
    {
        builder.Services.AddKeyedTransient<IIntegrationEventHandler, TIntegrationEventHandler>(typeof(TIntegrationEvent));
        builder.Services.Configure<EventBusSubscriptionOptions>(c =>
        {
            var eventType = typeof(TIntegrationEvent);
            var eventKey = eventType.GetCustomAttribute<IntegrationEventKeyAttribute>()?.Key;
            
            c.Subscriptions.TryAdd(eventKey ?? eventType.Name, eventType);
        });
        
        return builder;
    }
}
