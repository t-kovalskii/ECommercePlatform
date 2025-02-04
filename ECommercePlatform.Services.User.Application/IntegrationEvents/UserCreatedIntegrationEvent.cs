using ECommercePlatform.Shared.EventBus.Attributes;
using ECommercePlatform.Shared.EventBus.Events;

namespace ECommercePlatform.Services.User.Application.IntegrationEvents;

[IntegrationEventKey("user.created")]
public class UserCreatedIntegrationEvent : IntegrationEvent
{
    public required Guid UserId { get; init; }
}
