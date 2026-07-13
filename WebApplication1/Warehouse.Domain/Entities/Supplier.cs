using Warehouse.Domain.Exceptions;

namespace Warehouse.Domain.Entities;

public class Supplier
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string ContactEmail { get; private set; }

    public bool IsActive { get; private set; }


    public ICollection<Product> Products { get; private set; }
        = new List<Product>();


    private Supplier()
    {
    }


    public Supplier(
        string name,
        string contactEmail)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleException(
                "Supplier name required"
            );

        Id = Guid.NewGuid();

        Name = name;

        ContactEmail = contactEmail;

        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }
}