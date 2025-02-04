using ECommercePlatform.Services.User.Domain.DomainEvents;
using ECommercePlatform.Services.User.Domain.Exceptions;
using ECommercePlatform.Services.User.Domain.Models.Enums;

using ECommercePlatform.Shared.Utils.DataAccess;

namespace ECommercePlatform.Services.User.Domain.Models;

public class User : Entity
{
    public User(Guid merchantId, string firstName, string lastName, string email, string phoneNumber, string address)
    {
        MerchantId = merchantId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        
        AddDomainEvent(new UserCreatedDomainEvent(this));
    }
    
    public Guid MerchantId { get; private set; }
    
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public string Email { get; private set; }
    
    public string PhoneNumber { get; private set; }
    
    public string Address { get; private set; }

    public UserStatus Status { get; private set; } = UserStatus.Active;

    public void SetDeleted()
    {
        if (Status == UserStatus.Deleted)
        {
            throw new UserServiceDomainException($"User with id {Id} is already deleted.");
        }
        
        Status = UserStatus.Deleted;
        AddDomainEvent(new UserDeletedDomainEvent(Id));
    }
}
