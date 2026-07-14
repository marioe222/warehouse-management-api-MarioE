namespace Warehouse.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Sku { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public int QuantityInStock { get; private set; }

    public string? SupplierName { get; private set; }

    public DateTime? ExpiryDate { get; private set; }

    public bool IsArchived { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime LastUpdatedAt { get; private set; }


    // Foreign Key
    public Guid? SupplierId { get; private set; }


    // Navigation Property
    public Supplier? Supplier { get; private set; }


    // Navigation Property
    public ICollection<ProductImage> Images { get; private set; }
        = new List<ProductImage>();


    private Product()
    {
    }


    public Product(
        string name,
        string sku,
        string description,
        decimal price,
        int quantityInStock,
        string? supplierName,
        DateTime? expiryDate
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new Exception("Product name required");

        if (string.IsNullOrWhiteSpace(sku))
            throw new Exception("SKU required");

        if (price <= 0)
            throw new Exception("Price must be greater than zero");

        if (quantityInStock < 0)
            throw new Exception("Quantity cannot be negative");


        Id = Guid.NewGuid();

        Name = name;

        Sku = sku;

        Description = description;

        Price = price;

        QuantityInStock = quantityInStock;

        SupplierName = supplierName;

        ExpiryDate = expiryDate;

        IsArchived = false;

        CreatedAt = DateTime.UtcNow;

        LastUpdatedAt = DateTime.UtcNow;
    }


    public void UpdatePrice(decimal price)
    {
        if (IsArchived)
            throw new Exception("Archived product cannot update");

        if (price <= 0)
            throw new Exception("Invalid price");


        Price = price;

        LastUpdatedAt = DateTime.UtcNow;
    }


    public void UpdateQuantity(int quantity)
    {
        if (quantity < 0)
            throw new Exception("Invalid quantity");


        QuantityInStock = quantity;

        LastUpdatedAt = DateTime.UtcNow;
    }


    public void AssignSupplier(Supplier supplier)
    {
        if (IsArchived)
            throw new Exception("Archived product cannot be assigned");


        if (!supplier.IsActive)
            throw new Exception("Inactive supplier cannot be assigned");


        SupplierId = supplier.Id;

        SupplierName = supplier.Name;

        Supplier = supplier;

        LastUpdatedAt = DateTime.UtcNow;
    }


    public void Archive()
    {
        IsArchived = true;

        LastUpdatedAt = DateTime.UtcNow;
    }
}