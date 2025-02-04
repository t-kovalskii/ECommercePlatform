namespace ECommercePlatform.Shared.Utils.Mapper;

public interface IModelMapper
{
    T Map<T>(object source);

    IEnumerable<T> Map<T>(IEnumerable<object> sources);
    
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}
