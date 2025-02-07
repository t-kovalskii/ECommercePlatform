using ECommercePlatform.Shared.Utils.DataAccess;
using ECommercePlatform.Shared.Utils.Entity;

using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Services.User.Infrastructure.Context;

public partial class ECommerceUsersContext : IUnitOfWork
{
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving entities");
        
        await DispatchDomainEventsAsync();
        await base.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Entities saved");
        
        return true;
    }

    private async Task DispatchDomainEventsAsync()
    {
        _logger.LogInformation("Dispatching domain events");
        
        var registeredAggregates = Aggregate.GetRegisteredAggregates().ToList();
        while (registeredAggregates.Count != 0)
        {
            foreach (var aggregate in registeredAggregates)
            {
                var aggregateDomainEvents = aggregate.DomainEvents.ToList();
                
                Aggregate.RemoveAggregate(aggregate);
                
                foreach (var aggregateDomainEvent in aggregateDomainEvents)
                {
                    await _domainEventDispatcher.DispatchAsync(aggregateDomainEvent);
                }
            }
            
            registeredAggregates = Aggregate.GetRegisteredAggregates().ToList();
        }
        
        Aggregate.ClearRegisteredAggregates();
        
        _logger.LogInformation("Domain events dispatched");
    }
}
