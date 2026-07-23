using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Warehouse.IntegrationEvents.Events;
using Warehouse.Notifications.Api.Data;
using Warehouse.Notifications.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Notifications.Api.Services;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqConsumer> _logger;


    public RabbitMqConsumer(
        IServiceProvider serviceProvider,
        ILogger<RabbitMqConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "admin",
                    Password = "admin123"
                };


                var connection =
                    await factory.CreateConnectionAsync();


                var channel =
                    await connection.CreateChannelAsync();


                // Exchange used by Warehouse.Presentation
                await channel.ExchangeDeclareAsync(
                    exchange: "warehouse.events",
                    type: ExchangeType.Topic,
                    durable: true);



                // Notification queue
                await channel.QueueDeclareAsync(
                    queue: "warehouse.notifications",
                    durable: true,
                    exclusive: false,
                    autoDelete: false);



                // Connect queue with exchange
                await channel.QueueBindAsync(
                    queue: "warehouse.notifications",
                    exchange: "warehouse.events",
                    routingKey: "file.uploaded");



                _logger.LogInformation(
                    "RabbitMQ consumer connected and waiting for events");



                var consumer =
                    new AsyncEventingBasicConsumer(channel);



                consumer.ReceivedAsync += async (_, args) =>
                {
                    try
                    {
                        var body =
                            args.Body.ToArray();


                        var json =
                            Encoding.UTF8.GetString(body);



                        var fileUploaded =
                            JsonSerializer.Deserialize<WarehouseFileUploaded>(
                                json);



                        if (fileUploaded != null)
                        {
                            await SaveNotification(fileUploaded);
                        }



                        await channel.BasicAckAsync(
                            args.DeliveryTag,
                            false);
                    }
                    catch(Exception ex)
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
            catch(Exception ex)
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




    private async Task SaveNotification(
        WarehouseFileUploaded file)
    {

        using var scope =
            _serviceProvider.CreateScope();



        var db =
            scope.ServiceProvider
            .GetRequiredService<NotificationDbContext>();



        // Prevent duplicate notifications
        var exists =
            await db.Notifications
            .AnyAsync(x =>
                x.EventId == file.EventId.ToString());



        if (exists)
        {
            _logger.LogInformation(
                "Duplicate event ignored: {EventId}",
                file.EventId);

            return;
        }



        var notification = new Notification
        {
            Id = Guid.NewGuid(),

            EventId = file.EventId.ToString(),

            Message =
                $"File uploaded: {file.FileName}",

            CreatedAt =
                DateTime.UtcNow
        };



        db.Notifications.Add(notification);


        await db.SaveChangesAsync();



        _logger.LogInformation(
            "Notification created for file {FileName}",
            file.FileName);
    }
}