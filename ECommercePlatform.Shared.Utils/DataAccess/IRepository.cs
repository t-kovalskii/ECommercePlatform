using System.Linq.Expressions;

namespace ECommercePlatform.Shared.Utils.DataAccess;

public interface IRepository<T> where T : Entity
{
    IUnitOfWork UnitOfWork { get; }

    Task<IEnumerable<T>> GetAllAsync();
    
    Task<T> GetByIdAsync(Guid id);
    
    Task<Guid> AddAsync(T entity);
    
    Task<T> UpdateAsync(T entity);
    
    Task<bool> DeleteAsync(Guid id);
    
    Task<bool> ExistsAsync(Guid id);
    
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
}
