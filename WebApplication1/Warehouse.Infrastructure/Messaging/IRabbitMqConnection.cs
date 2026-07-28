using RabbitMQ.Client;

namespace Warehouse.Infrastructure.Messaging;

public interface IRabbitMqConnection : IAsyncDisposable
{
    Task<IChannel> CreateChannelAsync();
}