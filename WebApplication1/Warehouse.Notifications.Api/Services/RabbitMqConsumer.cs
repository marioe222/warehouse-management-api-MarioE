using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Warehouse.Infrastructure.Messaging;
using Warehouse.IntegrationEvents.Events;
using Warehouse.Notifications.Api.Data;
using Warehouse.Notifications.Api.Models;

namespace Warehouse.Notifications.Api.Services;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly IRabbitMqConnection _connection;
    private readonly RabbitMqOptions _options;

    public RabbitMqConsumer(
        IServiceProvider serviceProvider,
        ILogger<RabbitMqConsumer> logger,
        IRabbitMqConnection connection,
        IOptions<RabbitMqOptions> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _connection = connection;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var channel =
                    await _connection.CreateChannelAsync();

                await channel.ExchangeDeclareAsync(
                    exchange: _options.ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true);

                await channel.QueueDeclareAsync(
                    queue: "warehouse.notifications",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);

                await channel.QueueBindAsync(
                    queue: "warehouse.notifications",
                    exchange: _options.ExchangeName,
                    routingKey: "file.uploaded");

                await channel.QueueBindAsync(
                    queue: "warehouse.notifications",
                    exchange: _options.ExchangeName,
                    routingKey: "stock.low");

                _logger.LogInformation(
                    "RabbitMQ consumer connected.");

                var consumer =
                    new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (_, args) =>
                {
                    try
                    {
                        var json =
                            Encoding.UTF8.GetString(
                                args.Body.ToArray());

                        switch (args.RoutingKey)
                        {
                            case "file.uploaded":

                                var file =
                                    JsonSerializer.Deserialize<WarehouseFileUploaded>(json);

                                if (file != null)
                                    await SaveFileNotification(file);

                                break;

                            case "stock.low":

                                var stock =
                                    JsonSerializer.Deserialize<StockLowDetected>(json);

                                if (stock != null)
                                    await SaveStockLowNotification(stock);

                                break;
                        }

                        await channel.BasicAckAsync(
                            args.DeliveryTag,
                            false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Error processing RabbitMQ message");

                        await channel.BasicNackAsync(
                            args.DeliveryTag,
                            false,
                            true);
                    }
                };

                await channel.BasicConsumeAsync(
                    queue: "warehouse.notifications",
                    autoAck: false,
                    consumer: consumer);

                await Task.Delay(
                    Timeout.Infinite,
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "RabbitMQ unavailable. Retrying...");

                await Task.Delay(
                    5000,
                    stoppingToken);
            }
        }
    }

    private async Task SaveFileNotification(
        WarehouseFileUploaded file)
    {
        using var scope =
            _serviceProvider.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<NotificationDbContext>();

        var exists =
            await db.Notifications.AnyAsync(x =>
                x.EventId == file.EventId.ToString());

        if (exists)
        {
            _logger.LogInformation(
                "Duplicate file event ignored: {EventId}",
                file.EventId);

            return;
        }

        db.Notifications.Add(
            new Notification
            {
                Id = Guid.NewGuid(),
                EventId = file.EventId.ToString(),
                Message = $"File uploaded: {file.FileName}",
                CreatedAt = DateTime.UtcNow
            });

        await db.SaveChangesAsync();

        _logger.LogInformation(
            "File upload notification saved.");
    }

    private async Task SaveStockLowNotification(
        StockLowDetected stock)
    {
        using var scope =
            _serviceProvider.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<NotificationDbContext>();

        var exists =
            await db.Notifications.AnyAsync(x =>
                x.EventId == stock.EventId.ToString());

        if (exists)
        {
            _logger.LogInformation(
                "Duplicate stock event ignored: {EventId}",
                stock.EventId);

            return;
        }

        db.Notifications.Add(
            new Notification
            {
                Id = Guid.NewGuid(),
                EventId = stock.EventId.ToString(),
                Message = $"LOW STOCK ALERT: {stock.ProductName} has {stock.CurrentQuantity} items remaining.",
                CreatedAt = DateTime.UtcNow
            });

        await db.SaveChangesAsync();

        _logger.LogInformation(
            "Stock notification saved.");
    }
}