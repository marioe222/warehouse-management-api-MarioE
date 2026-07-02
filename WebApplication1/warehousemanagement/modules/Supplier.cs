namespace warehousemanagement.modules;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
}