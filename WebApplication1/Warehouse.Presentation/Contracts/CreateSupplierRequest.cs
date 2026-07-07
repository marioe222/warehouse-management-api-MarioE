namespace Warehouse.Presentation.Contracts;

public class CreateSupplierRequest
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string ContactEmail { get; set; }
    public required string PhoneNumber { get; set; }
}