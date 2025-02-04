using ECommercePlatform.Shared.Utils.Async;

using RabbitMQ.Client;

namespace ECommercePlatform.Shared.EventBus.Services;

public class RabbitMqConnectionKeeper : IRabbitMqConnectionKeeper, IDisposable
{
    private static AsyncLazy<IConnection> _connectionLazy;

    public RabbitMqConnectionKeeper(IRabbitMqConnectionFactory connectionFactory)
    {
        _connectionLazy = new AsyncLazy<IConnection>(async () => await connectionFactory.EstablishConnectionAsync());
    }
    
    public Task<IConnection> Connection => _connectionLazy.Value;

    public void Dispose()
    {
        if (_connectionLazy.IsValueCreated)
        {
            _connectionLazy.Value.Dispose();
        }
    }
}