namespace Warehouse.Presentation.modules;

public class Products
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public required string SupplierName { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
}