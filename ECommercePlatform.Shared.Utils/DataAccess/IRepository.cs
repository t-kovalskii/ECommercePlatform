using System.Linq.Expressions;
using ECommercePlatform.Shared.Utils.Entity;

namespace ECommercePlatform.Shared.Utils.DataAccess;

public interface IRepository<T> where T : Aggregate
{
    IUnitOfWork UnitOfWork { get; }

    Task<IEnumerable<T>> GetAllAsync();
    
    Task<T?> GetByIdAsync(Guid id);
    
    Task<Guid> AddAsync(T entity);
    
    Task<T> UpdateAsync(T entity);
    
    Task<bool> DeleteAsync(T entity);
    
    Task<bool> ExistsAsync(Guid id);
}
