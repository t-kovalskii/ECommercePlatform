using MediatR;

namespace ECommercePlatform.Shared.Utils.Events;

public abstract class DomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    public abstract Task Handle(TDomainEvent notification, CancellationToken cancellationToken);
}