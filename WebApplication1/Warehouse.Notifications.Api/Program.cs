using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Messaging;
using Warehouse.Notifications.Api.Data;
using Warehouse.Notifications.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("NotificationDb")
    ));

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();

builder.Services.AddHostedService<RabbitMqConsumer>();

var app = builder.Build();

app.MapControllers();

app.Run();