using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Contracts;

public class UpdateProductQuantityRequest
{
    [Required]
    [Range(0, int.MaxValue)]
    public int QuantityInStock { get; set; }
}