namespace Warehouse.Domain.Entities;

public class Supplier
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string ContactEmail { get; private set; }

    public bool IsActive { get; private set; }


    public Supplier(
        string name,
        string contactEmail)
    {
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