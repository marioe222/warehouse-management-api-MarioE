namespace Warehouse.Domain.Entities;

public class Product
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Sku { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public int QuantityInStock { get; private set; }

    public int SupplierId { get; private set; }

    public string SupplierName { get; private set; } = string.Empty;

    public DateTime? ExpiryDate { get; private set; }

    public bool IsArchived { get; private set; }


    public Product(
        string name,
        string sku,
        string description,
        decimal price,
        int quantityInStock,
        int supplierId,
        string supplierName,
        DateTime? expiryDate)
    {
        Name = name;
        Sku = sku;
        Description = description;
        Price = price;
        QuantityInStock = quantityInStock;
        SupplierId = supplierId;
        SupplierName = supplierName;
        ExpiryDate = expiryDate;
        IsArchived = false;
    }


    public void UpdatePrice(decimal price)
    {
        Price = price;
    }

    public void UpdateQuantity(int quantity)
    {
        QuantityInStock = quantity;
    }

    public void AssignSupplier(int supplierId, string supplierName)
    {
        SupplierId = supplierId;
        SupplierName = supplierName;
    }

    public void Archive()
    {
        IsArchived = true;
    }
}