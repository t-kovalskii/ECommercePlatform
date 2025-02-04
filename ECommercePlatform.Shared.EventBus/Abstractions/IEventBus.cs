using ECommercePlatform.Shared.EventBus.Events;

namespace ECommercePlatform.Shared.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
}