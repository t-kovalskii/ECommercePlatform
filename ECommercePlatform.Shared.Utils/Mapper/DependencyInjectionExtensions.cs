using Microsoft.Extensions.DependencyInjection;

namespace ECommercePlatform.Shared.Utils.Mapper;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMapper<TModelMapperFactory>(this IServiceCollection services)
        where TModelMapperFactory : class, IModelMapperFactory
    {
        services.AddSingleton<IModelMapper, ModelMapper>();
        services.AddSingleton<IModelMapperFactory, TModelMapperFactory>();
        
        return services;
    }
}
