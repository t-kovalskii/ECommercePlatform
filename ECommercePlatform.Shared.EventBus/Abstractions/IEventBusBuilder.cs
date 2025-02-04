using Microsoft.Extensions.DependencyInjection;

namespace ECommercePlatform.Shared.EventBus.Abstractions;

public interface IEventBusBuilder
{
    IServiceCollection Services { get; }
}
