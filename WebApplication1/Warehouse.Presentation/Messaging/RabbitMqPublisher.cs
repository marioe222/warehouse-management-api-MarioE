using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Warehouse.Presentation.Messaging;

public class RabbitMqPublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public async Task PublishAsync<T>(
        T message,
        string routingKey)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin123"
        };

        await using var connection = await factory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();


        await channel.ExchangeDeclareAsync(
            exchange: "warehouse.events",
            type: ExchangeType.Topic,
            durable: true);


        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message));


        await channel.BasicPublishAsync(
            exchange: "warehouse.events",
            routingKey: routingKey,
            body: body);
    }
}