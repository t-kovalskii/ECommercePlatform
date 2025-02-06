using System.Reflection;
using ECommercePlatform.Shared.Utils.Behaviours;
using ECommercePlatform.Shared.Utils.Events;

using Microsoft.Extensions.DependencyInjection;

namespace ECommercePlatform.Services.User.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDefaultBehaviours();
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}
