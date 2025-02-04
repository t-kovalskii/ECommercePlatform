using ECommercePlatform.Shared.Utils.DataAccess;

namespace ECommercePlatform.Services.User.Domain.Models;

public class Address(
    Guid userId,
    string street,
    string city,
    string state,
    string zipCode,
    string country,
    bool isPrimary) : Entity
{
    public Guid UserId { get; } = userId;
    
    public string Street { get; } = street;
    
    public string City { get; } = city;
    
    public string State { get; } = state;
    
    public string ZipCode { get; } = zipCode;
    
    public string Country { get; } = country;
    
    public bool IsPrimary { get; } = isPrimary;
}
