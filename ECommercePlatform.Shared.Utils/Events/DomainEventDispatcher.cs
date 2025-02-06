using MediatR;

namespace ECommercePlatform.Shared.Utils.Events;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public Task DispatchAsync(DomainEvent domainEvent)
    {
        return mediator.Publish(domainEvent);
    }
}
