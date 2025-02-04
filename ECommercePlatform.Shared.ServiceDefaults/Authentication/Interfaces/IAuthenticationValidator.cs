using System.Security.Claims;

namespace ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;

public interface IAuthenticationValidator
{
    Task<ClaimsPrincipal?> ValidateAsync(string credentials);
}