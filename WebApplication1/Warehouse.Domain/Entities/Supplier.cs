namespace Warehouse.Domain.Entities;

public class Supplier
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public string ContactEmail { get; private set; }

    public bool IsActive { get; private set; }


    public Supplier(
        string name,
        string contactEmail)
    {
        Name = name;
        ContactEmail = contactEmail;
        IsActive = true;
    }


    public void Deactivate()
    {
        IsActive = false;
    }


    public void Activate()
    {
        IsActive = true;
    }
}