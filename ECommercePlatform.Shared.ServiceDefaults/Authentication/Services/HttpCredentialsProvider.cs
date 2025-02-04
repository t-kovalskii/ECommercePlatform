using ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ECommercePlatform.Shared.ServiceDefaults.Authentication.Services;

public class HttpCredentialsProvider(IHttpContextAccessor httpContextAccessor) : ICredentialsProvider
{
    public string Credentials
    {
        get
        {
            var authHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString() ?? string.Empty;
            var split = authHeader.Split(" ");

            var token = split.Length > 1 ? split[1] : split[0];

            return token.Trim();
        }
    }
}
