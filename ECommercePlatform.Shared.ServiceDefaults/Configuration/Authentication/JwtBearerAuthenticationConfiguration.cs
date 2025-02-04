namespace ECommercePlatform.Shared.ServiceDefaults.Configuration.Authentication;

public class JwtBearerAuthenticationConfiguration
{
    public string SigningKey { get; }
    
    public int ExpiryHours { get; }
}
