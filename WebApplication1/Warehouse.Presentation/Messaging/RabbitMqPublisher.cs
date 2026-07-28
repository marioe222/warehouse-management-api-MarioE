using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Warehouse.Application.Interfaces;
using Warehouse.Infrastructure.Messaging;
using RabbitMQ.Client;

namespace Warehouse.Presentation.Messaging;

public class RabbitMqPublisher : IEventPublisher
{
    private readonly IRabbitMqConnection _connection;
    private readonly RabbitMqOptions _options;

    public RabbitMqPublisher(
        IRabbitMqConnection connection,
        IOptions<RabbitMqOptions> options)
    {
        _connection = connection;
        _options = options.Value;
    }

    public async Task PublishAsync<T>(
        T message,
        string routingKey)
    {
        var channel =
            await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true);

        var body =
            Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            body: body);

        await channel.DisposeAsync();
    }
}