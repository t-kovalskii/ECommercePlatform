using System.Security.Claims;

namespace ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;

public interface IUserInfoProvider
{
    void SetPrincipal(ClaimsPrincipal principal);
    
    ClaimsPrincipal Principal { get; }
}
