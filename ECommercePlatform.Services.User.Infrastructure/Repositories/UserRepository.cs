using ECommercePlatform.Shared.Utils.DataAccess;
using ECommercePlatform.Services.User.Infrastructure.Context;
using ECommercePlatform.Shared.Utils.Mapper;

using Microsoft.EntityFrameworkCore;

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
        var user = await eCommerceUsersContext.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == id);
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

    public async Task<Domain.Models.User> UpdateAsync(Domain.Models.User entity)
    {
        var userEntity = await eCommerceUsersContext.Users.Include(u => u.Addresses)
            .FirstOrDefaultAsync(u => u.Id == entity.Id);
        
        if (userEntity is null) 
        {
            throw new Exception($"User with id {entity.Id} not found");
        }
        
        modelMapper.Map(entity, userEntity);
        
        return entity;
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