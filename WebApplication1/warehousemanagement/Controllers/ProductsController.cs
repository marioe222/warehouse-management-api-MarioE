using Microsoft.AspNetCore.Mvc;
using warehousemanagement.Data;
using warehousemanagement.Contracts;
using System.Globalization;
using warehousemanagement.modules;
using warehousemanagement.Services;

namespace warehousemanagement.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service = new();

        // 1 GET /api/products
        [HttpGet]
        public IActionResult GetAll([FromQuery] bool onlyAvailable = false)
        {
            return Ok(_service.GetAll(onlyAvailable));
        }

        // 2 GET /api/products/{id}
        [HttpGet("{id:guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var product = _service.GetById(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // 3 SEARCH (k)
        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] string? name,
            [FromQuery] string? supplier)
        {
            if (string.IsNullOrWhiteSpace(name) &&
                string.IsNullOrWhiteSpace(supplier))
            {
                return BadRequest("At least one search parameter is required.");
            }

            var query = FakeWarehouseStore.Products
                .Where(p => !p.IsArchived);

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p =>
                    p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(supplier))
            {
                query = query.Where(p =>
                    p.SupplierName.Contains(supplier, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(query.ToList());
        }

        // 4 CREATE ()
        [HttpPost]
        public IActionResult Create([FromBody] CreateProductRequest request)
        {
            var skuExists = FakeWarehouseStore.Products
                .Any(p => p.Sku == request.Sku);

            if (skuExists)
                return BadRequest("SKU already exists.");

            var product = new Products
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Sku = request.Sku,
                Description = request.Description,
                Price = request.Price,
                QuantityInStock = request.QuantityInStock,
                SupplierName = request.SupplierName,
                ExpiryDate = request.ExpiryDate,
                IsArchived = false,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            FakeWarehouseStore.Products.Add(product);

            return CreatedAtAction(nameof(GetById),
                new { id = product.Id }, product);
        }

        // 5 UPDATE QUANTITY ()
        [HttpPost("{id:guid}/quantity")]
        public IActionResult UpdateQuantity(Guid id, [FromBody] UpdateProductQuantityRequest request)
        {
            if (request.QuantityInStock < 0)
                return BadRequest("Quantity cannot be negative.");

            var success = _service.UpdateQuantity(id, request.QuantityInStock);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // 6 UPDATE PRICE ()
        [HttpPost("{id:guid}/price")]
        public IActionResult UpdatePrice(Guid id, [FromBody] UpdateProductPriceRequest request)
        {
            if (request.Price <= 0)
                return BadRequest("Price must be greater than zero.");

            var success = _service.UpdatePrice(id, request.Price);

            if (!success)
                return NotFound();

            Console.WriteLine($"Price change for {id} → {request.Price}");

            return NoContent();
        }

        // 7 IMAGE UPLOAD ()
        [HttpPost("{id:guid}/image")]
        [Consumes("multipart/form-data")]
        public IActionResult UploadImage(Guid id, [FromForm] IFormFile file)
        {
            var product = FakeWarehouseStore.Products
                .FirstOrDefault(p => p.Id == id && !p.IsArchived);

            if (product == null)
                return NotFound();

            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            if (file.Length > 2 * 1024 * 1024)
                return BadRequest("Max file size is 2MB.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".jpg" && extension != ".png")
                return BadRequest("Only JPG and PNG allowed.");

            var uploadsPath = Path.Combine("wwwroot", "uploads");
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return Ok(new { FileName = fileName });
        }

        // 8 DELETE
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var product = FakeWarehouseStore.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            product.IsArchived = true;
            product.LastUpdatedAt = DateTime.UtcNow;

            return NoContent();
        }

        // 9 SERVER TIME
        [HttpGet("server-time")]
        public IActionResult GetServerTime(
            [FromHeader(Name = "Accept-Language")] string language)
        {
            var culture = language switch
            {
                "fr-FR" => new CultureInfo("fr-FR"),
                "ar-LB" => new CultureInfo("ar-LB"),
                _ => new CultureInfo("en-US")
            };

            var formattedDate = DateTime.UtcNow.ToString("F", culture);

            return Ok(formattedDate);
        }

        // 10 ASSIGN SUPPLIER
        [HttpPost("{id}/assign-supplier/{supplierId}")]
        public IActionResult AssignSupplier(Guid id, Guid supplierId)
        {
            var product = FakeWarehouseStore.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            if (product.IsArchived)
                return BadRequest("Archived product cannot be assigned");

            var supplier = FakeWarehouseStore.Suppliers
                .FirstOrDefault(s => s.Id == supplierId);

            if (supplier == null || !supplier.IsActive)
                return NotFound("Supplier not found or inactive");

            
            product.SupplierName = supplier.Name;
            product.LastUpdatedAt = DateTime.UtcNow;

            return NoContent();
        }
    }
}