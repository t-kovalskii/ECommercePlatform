namespace ECommercePlatform.Shared.Utils.Entity;

public abstract class BaseEntity
{
    public Guid Id { get; } = Guid.NewGuid();
}
