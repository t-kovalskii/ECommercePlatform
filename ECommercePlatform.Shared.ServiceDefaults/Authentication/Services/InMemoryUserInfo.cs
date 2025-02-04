using ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;

using System.Security.Claims;

namespace ECommercePlatform.Shared.ServiceDefaults.Authentication.Services;

public class InMemoryUserInfo : IUserInfo
{
    public void SetPrincipal(ClaimsPrincipal principal)
    {
        Principal = principal;
    }

    public ClaimsPrincipal Principal { get; private set; } = new();
}
