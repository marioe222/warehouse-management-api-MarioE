using Microsoft.EntityFrameworkCore;
using Warehouse.Notifications.Api.Models;

namespace Warehouse.Notifications.Api.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(
        DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }


    public DbSet<Notification> Notifications => Set<Notification>();
}