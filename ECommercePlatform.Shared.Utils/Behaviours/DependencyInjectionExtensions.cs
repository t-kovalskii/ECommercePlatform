using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace ECommercePlatform.Shared.Utils.Behaviours;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDefaultBehaviours(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
        });

        return services;
    }
}