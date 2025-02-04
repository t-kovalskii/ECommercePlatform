using ECommercePlatform.Shared.Utils.Events;

namespace ECommercePlatform.Services.User.Domain.DomainEvents;

public class UserDeletedDomainEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; set; } = userId;
}
