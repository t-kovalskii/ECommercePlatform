using ECommercePlatform.Shared.Utils.DataAccess;
using ECommercePlatform.Shared.Utils.Entity;

namespace ECommercePlatform.Services.User.Domain.Models;

public class Address(
    string street,
    string city,
    string state,
    string zipCode,
    string country) : BaseEntity
{
    public Guid UserId { get; }
    
    public string Street { get; } = street;
    
    public string City { get; } = city;
    
    public string State { get; } = state;
    
    public string ZipCode { get; } = zipCode;
    
    public string Country { get; } = country;
}
