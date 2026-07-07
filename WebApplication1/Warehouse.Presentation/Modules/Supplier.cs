namespace Warehouse.Presentation.modules;

public class Supplier
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string ContactEmail { get; set; }
    public required string PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
}