using ECommercePlatform.Shared.ServiceDefaults.Authentication.Interfaces;
using ECommercePlatform.Shared.ServiceDefaults.Authentication.Services;

using Microsoft.Extensions.DependencyInjection;

namespace ECommercePlatform.Shared.ServiceDefaults.Authentication;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDefaultServiceAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IAuthenticationValidator, JwtAuthenticationValidator>();
        
        services.AddScoped<ICredentialsProvider, HttpCredentialsProvider>();
        services.AddScoped<IUserInfoProvider, InMemoryUserInfoProvider>();

        return services;
    }

    public static IServiceCollection
        AddCustomServiceAuthentication<TValidator, TCredentialsProvider, TUserInfoProvider>(this IServiceCollection services)
        where TValidator : class, IAuthenticationValidator
        where TCredentialsProvider : class, ICredentialsProvider
        where TUserInfoProvider : class, IUserInfoProvider
    {
        services.AddSingleton<IAuthenticationValidator, TValidator>();
        
        services.AddScoped<ICredentialsProvider, TCredentialsProvider>();
        services.AddScoped<IUserInfoProvider, TUserInfoProvider>();
        
        return services;
    }
}
