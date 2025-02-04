using ECommercePlatform.Services.User.Domain.DomainEvents;
using ECommercePlatform.Shared.Utils.Events;

using Microsoft.Extensions.Logging;

namespace ECommercePlatform.Services.User.Application.DomainEventHandlers;

public class UserDeletedDomainEventHandler(ILogger logger) : DomainEventHandler<UserDeletedDomainEvent>
{
    public override Task Handle(UserDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("User {UserId} deleted", notification.UserId);
        return Task.CompletedTask;
    }
}