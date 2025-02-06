using ECommercePlatform.Services.User.Infrastructure.Mappings;
using ECommercePlatform.Services.User.Infrastructure.Repositories;

using ECommercePlatform.Shared.ServiceDefaults.Authentication;

using ECommercePlatform.Shared.Utils.DataAccess;
using ECommercePlatform.Shared.Utils.Mapper;

using Microsoft.Extensions.DependencyInjection;

namespace ECommercePlatform.Services.User.Infrastructure;

public static class DependencyInjectionExtesnions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddDefaultServiceAuthentication();
        services.AddMapper<UserServiceModelMapperFactory>();
        
        services.AddScoped<IRepository<Domain.Models.User>, UserRepository>();
        
        return services;
    }
}
