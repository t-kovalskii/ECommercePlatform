using RabbitMQ.Client;

namespace ECommercePlatform.Shared.EventBus.Services;

public interface IRabbitMqConnectionKeeper
{
    Task<IConnection> Connection { get; }
}
