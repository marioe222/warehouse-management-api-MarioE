namespace Warehouse.Presentation.Contracts;

public class CreateProductRequest
{
    public required string  Name { get; set; }
    public required string Sku { get; set; }
    public required string Description { get; set; }
    public decimal  Price { get; set; }
    public int QuantityInStock { get; set; }
    public required string SupplierName { get; set; }
    public DateTime ExpiryDate { get; set; }
}