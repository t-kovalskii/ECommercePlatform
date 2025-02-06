using ECommercePlatform.Shared.Utils.Mapper;

using Microsoft.Extensions.Configuration;

using AutoMapper;

namespace ECommercePlatform.Services.User.Infrastructure.Mappings;

public class UserServiceModelMapperFactory : IModelMapperFactory
{
    public IMapper CreateMapper(IConfiguration configuration)
    {
        var mapperConfiguration = new MapperConfiguration(ConfigureMap);
        return mapperConfiguration.CreateMapper();
    }

    private void ConfigureMap(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Domain.Models.User, Entities.User>();
        cfg.CreateMap<Domain.Models.Address, Entities.Address>();
    }
}
