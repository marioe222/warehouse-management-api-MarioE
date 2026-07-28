using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Warehouse.Infrastructure.Messaging;

public class RabbitMqConnection : IRabbitMqConnection
{
    private readonly RabbitMqOptions _options;

    private IConnection? _connection;

    public RabbitMqConnection(
        IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password
            };

            _connection = await factory.CreateConnectionAsync();
        }

        return await _connection.CreateChannelAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}