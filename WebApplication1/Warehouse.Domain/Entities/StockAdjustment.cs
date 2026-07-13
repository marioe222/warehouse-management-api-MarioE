namespace Warehouse.Domain.Entities;

public class StockAdjustment
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int QuantityChange { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    
}