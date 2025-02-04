using ECommercePlatform.Shared.ServiceDefaults.Configuration.Authentication;
using ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommercePlatform.Shared.ServiceDefaults.Authentication.Services;

public class JwtAuthenticationValidator(
    IOptions<JwtBearerAuthenticationConfiguration> configuration) : IAuthenticationValidator
{
    private readonly JwtBearerAuthenticationConfiguration _configuration = configuration.Value;
    
    public Task<ClaimsPrincipal?> ValidateAsync(string credentials)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = GetTokenValidationParameters();

        try
        {
            var principal = tokenHandler.ValidateToken(credentials, tokenValidationParameters, out _);
            return Task.FromResult<ClaimsPrincipal?>(principal);
        }
        catch
        {
            return Task.FromResult<ClaimsPrincipal?>(null);
        }
    }

    private TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SigningKey))
        };
    }
}
