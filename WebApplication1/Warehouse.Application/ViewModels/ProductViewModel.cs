namespace Warehouse.Application.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public bool IsArchived { get; set; }


    public Guid? SupplierId { get; set; }

    public string? SupplierName { get; set; }
}