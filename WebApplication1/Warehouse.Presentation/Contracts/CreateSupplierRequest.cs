using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Contracts;

public class CreateSupplierRequest
{
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;


    [Required] [MaxLength(100)] public string Country { get; set; } = string.Empty;


    [Required] [EmailAddress] public string ContactEmail { get; set; } = string.Empty;


    [Required] [Phone] public string PhoneNumber { get; set; } = string.Empty;
}