using MediatR;

using System.ComponentModel.DataAnnotations.Schema;
using ECommercePlatform.Shared.Utils.Events;

namespace ECommercePlatform.Shared.Utils.Entity;

public abstract class Aggregate : BaseEntity
{
    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = [];
    
    [NotMapped]
    private static readonly AsyncLocal<HashSet<Aggregate>> RegisteredAggregates = new();
    
    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Aggregate()
    {
        RegisteredAggregates.Value ??= new HashSet<Aggregate>();
    }

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
        RegisteredAggregates.Value?.Add(this);
    }
    
    public static IEnumerable<Aggregate> GetRegisteredAggregates() =>
        RegisteredAggregates.Value?.ToList() ?? [];
    
    public static void RemoveAggregate(Aggregate aggregate) => RegisteredAggregates.Value?.Remove(aggregate);
    
    public static void ClearRegisteredAggregates() => RegisteredAggregates.Value?.Clear();
}