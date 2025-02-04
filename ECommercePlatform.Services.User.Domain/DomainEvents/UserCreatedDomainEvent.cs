using ECommercePlatform.Shared.Utils.Events;

namespace ECommercePlatform.Services.User.Domain.DomainEvents;

public class UserCreatedDomainEvent(Models.User user) : DomainEvent
{
    public Models.User User { get; init; } = user;
}
