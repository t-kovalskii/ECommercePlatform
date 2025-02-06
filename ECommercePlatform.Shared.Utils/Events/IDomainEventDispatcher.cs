using ECommercePlatform.Shared.Utils.Entity;

namespace ECommercePlatform.Shared.Utils.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(DomainEvent domainEvent);
}
