using RabbitMQ.Client;

namespace ECommercePlatform.Shared.EventBus.Services;

public interface IRabbitMqConnectionFactory
{
    Task<IConnection> EstablishConnectionAsync();
}
