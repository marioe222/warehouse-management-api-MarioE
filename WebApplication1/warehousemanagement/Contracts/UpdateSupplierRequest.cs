namespace WarehouseManagement.Contracts;

public class UpdateSupplierRequest
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string ContactEmail { get; set; }
    public string PhoneNumber { get; set; }
}