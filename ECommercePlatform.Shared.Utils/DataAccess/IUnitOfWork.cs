namespace ECommercePlatform.Shared.Utils.DataAccess;

public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}
