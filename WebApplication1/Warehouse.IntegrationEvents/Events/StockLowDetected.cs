namespace Warehouse.IntegrationEvents.Events;

public class StockLowDetected
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int CurrentQuantity { get; set; }

    public int MinimumQuantity { get; set; }

    public DateTime DetectedAt { get; set; }
}