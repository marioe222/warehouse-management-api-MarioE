namespace Warehouse.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string SKU { get; private set; }

    public decimal Price { get; private set; }

    public int Quantity { get; private set; }

    public bool IsArchived { get; private set; }


    public Product(
        string name,
        string sku,
        decimal price,
        int quantity)
    {

        if(string.IsNullOrEmpty(name))
            throw new Exception("Product name required");


        if(string.IsNullOrEmpty(sku))
            throw new Exception("SKU required");


        if(price <= 0)
            throw new Exception("Price must be greater than zero");


        if(quantity < 0)
            throw new Exception("Quantity cannot be negative");


        Id = Guid.NewGuid();
        Name = name;
        SKU = sku;
        Price = price;
        Quantity = quantity;
    }


    public void UpdatePrice(decimal price)
    {
        if(IsArchived)
            throw new Exception("Archived product cannot update");


        if(price <=0)
            throw new Exception("Invalid price");


        Price = price;
    }
    
    public Guid? SupplierId { get; private set; }


    public void AssignSupplier(Supplier supplier)
    {
        if(IsArchived)
            throw new Exception(
                "Archived product cannot be assigned"
            );


        if(!supplier.IsActive)
            throw new Exception(
                "Inactive supplier cannot be assigned"
            );


        SupplierId = supplier.Id;
    }

    public void UpdateQuantity(int quantity)
    {
        if(quantity <0)
            throw new Exception("Invalid quantity");


        Quantity = quantity;
    }


    public void Archive()
    {
        IsArchived = true;
    }
}