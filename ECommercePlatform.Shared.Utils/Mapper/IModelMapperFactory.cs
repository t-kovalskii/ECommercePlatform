using Microsoft.Extensions.Configuration;

using AutoMapper;

namespace ECommercePlatform.Shared.Utils.Mapper;

public interface IModelMapperFactory
{
    IMapper CreateMapper(IConfiguration configuration);
}