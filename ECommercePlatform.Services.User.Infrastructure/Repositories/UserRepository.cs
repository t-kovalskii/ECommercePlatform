using ECommercePlatform.Shared.Utils.DataAccess;
using ECommercePlatform.Services.User.Infrastructure.Context;
using ECommercePlatform.Shared.Utils.Mapper;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace ECommercePlatform.Services.User.Infrastructure.Repositories;

public class UserRepository(
    ECommerceUsersContext eCommerceUsersContext,
    IModelMapper modelMapper) : IRepository<Domain.Models.User>
{
    public IUnitOfWork UnitOfWork => eCommerceUsersContext;
    
    public async Task<IEnumerable<Domain.Models.User>> GetAllAsync()
    {
        var userDtos = await eCommerceUsersContext.Users.ToListAsync();
        return modelMapper.Map<Domain.Models.User>(userDtos);
    }

    public async Task<Domain.Models.User?> GetByIdAsync(Guid id)
    {
        var user = await eCommerceUsersContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return null;
        }

        return modelMapper.Map<Domain.Models.User>(user);
    }

    public async Task<Guid> AddAsync(Domain.Models.User entity)
    {
        var newUser = modelMapper.Map<Entities.User>(entity);
        await eCommerceUsersContext.Users.AddAsync(newUser);
        
        return newUser.Id;
    }

    public Task<Domain.Models.User> UpdateAsync(Domain.Models.User entity)
    {
        var entityToUpdate = modelMapper.Map<Entities.User>(entity);
        eCommerceUsersContext.Users.Update(entityToUpdate);
        
        return Task.FromResult(entity);
    }

    public Task<bool> DeleteAsync(Domain.Models.User user)
    {
        var userToDelete = modelMapper.Map<Entities.User>(user);
        eCommerceUsersContext.Users.Remove(userToDelete);
        
        return Task.FromResult(true);
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        var user = eCommerceUsersContext.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user is not null);
    }
}