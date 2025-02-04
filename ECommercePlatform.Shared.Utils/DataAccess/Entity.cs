using MediatR;

namespace ECommercePlatform.Shared.Utils.DataAccess;

public abstract class Entity
{
    public Guid Id { get; } = Guid.NewGuid();
    
    private readonly List<INotification> _domainEvents = [];
    
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}
