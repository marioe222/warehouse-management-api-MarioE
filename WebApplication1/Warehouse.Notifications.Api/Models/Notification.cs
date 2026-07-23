namespace Warehouse.Notifications.Api.Models;

public class Notification
{
    public Guid Id { get; set; }

    public string EventId { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}