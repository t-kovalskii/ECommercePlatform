using ECommercePlatform.Services.User.Application.IntegrationEvents;
using ECommercePlatform.Services.User.Domain.DomainEvents;
using ECommercePlatform.Shared.EventBus.Abstractions;
using ECommercePlatform.Shared.Utils.Events;

using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Services.User.Application.DomainEventHandlers;

public class UserCreatedDomainEventHandler(
    ILogger logger,
    IEventBus eventBus) : DomainEventHandler<UserCreatedDomainEvent>
{
    public override Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName}. User Id: {UserId}",
            nameof(UserCreatedDomainEventHandler),
            notification.User.Id);

        var userId = notification.User.Id;
        
        logger.LogInformation("Publishing user created event for user '{UserId}'", userId);
        eventBus.PublishAsync(new UserCreatedIntegrationEvent { UserId = userId });
        
        return Task.CompletedTask;
    }
}
