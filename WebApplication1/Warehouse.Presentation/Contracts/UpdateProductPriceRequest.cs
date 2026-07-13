using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Contracts;

public class UpdateProductPriceRequest
{
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}