using Microsoft.EntityFrameworkCore;
using Warehouse.Notifications.Api.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("NotificationDb")
    ));


var app = builder.Build();


app.MapControllers();


app.Run();