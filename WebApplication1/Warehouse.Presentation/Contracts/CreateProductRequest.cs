using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Contracts;

public class CreateProductRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;


    [Required]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;


    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;


    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }


    [Range(0, int.MaxValue)]
    public int QuantityInStock { get; set; }


    [Required]
    [MaxLength(100)]
    public string SupplierName { get; set; } = string.Empty;


    [Required]
    public DateTime ExpiryDate { get; set; }
}