using ECommercePlatform.Services.User.Infrastructure.Entities;
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
        cfg.CreateMap<Address, Domain.Models.Address>().ReverseMap();

        cfg.CreateMap<Entities.User, Domain.Models.User>()
            .ConstructUsing(src => (Domain.Models.User)Activator.CreateInstance(typeof(Domain.Models.User), true)!)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Addresses.FirstOrDefault()))
            .ForMember(dest => dest.Status, opt => opt.Ignore());

        cfg.CreateMap<Domain.Models.User, Entities.User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Addresses, opt => 
                opt.MapFrom((src, dest, destMember, context) =>
                    src.Address != null 
                        ? [context.Mapper.Map<Address>(src.Address)]
                        : new List<Address>()));
    }
}
