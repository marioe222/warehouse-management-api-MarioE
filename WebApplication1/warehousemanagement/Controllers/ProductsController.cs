using Microsoft.AspNetCore.Mvc;
using warehousemanagement.Data;
using warehousemanagement.Contracts;
using System.Globalization;
using warehousemanagement.modules;

namespace warehousemanagement.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        // 1 GET /api/products
        [HttpGet]
        public IActionResult GetAll([FromQuery] bool onlyAvailable = false)
        {
            var products = FakeWarehouseStore.Products
                .Where(p => !p.IsArchived);

            if (onlyAvailable)
            {
                products = products.Where(p => p.QuantityInStock > 0);
            }

            var result = products
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return Ok(result);
        }

        // 2️ GET /api/products/{id}
        [HttpGet("{id:guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var product = FakeWarehouseStore.Products
                .FirstOrDefault(p => p.Id == id && !p.IsArchived);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // 3️ GET /api/products/search?name=&supplier=
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

        // 4️ POST /api/products
        [HttpPost]
        public IActionResult Create([FromBody] CreateProductRequest request)
        {
            var skuExists = FakeWarehouseStore.Products
                .Any(p => p.Sku == request.Sku);

            if (skuExists)
                return BadRequest("SKU already exists.");

            var product = new products
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

        // 5️ POST /api/products/{id}/quantity
        [HttpPost("{id:guid}/quantity")]
        public IActionResult UpdateQuantity(
            Guid id,
            [FromBody] UpdateProductQuantityRequest request)
        {
            if (request.QuantityInStock < 0)
                return BadRequest("Quantity cannot be negative.");

            var product = FakeWarehouseStore.Products
                .FirstOrDefault(p => p.Id == id && !p.IsArchived);

            if (product == null)
                return NotFound();

            product.QuantityInStock = request.QuantityInStock;
            product.LastUpdatedAt = DateTime.UtcNow;

            return NoContent();
        }

        // 6️ POST /api/products/{id}/price
        [HttpPost("{id:guid}/price")]
        public IActionResult UpdatePrice(
            Guid id,
            [FromBody] UpdateProductPriceRequest request)
        {
            if (request.Price <= 0)
                return BadRequest("Price must be greater than zero.");

            var product = FakeWarehouseStore.Products
                .FirstOrDefault(p => p.Id == id && !p.IsArchived);

            if (product == null)
                return NotFound();

            // audit log
            Console.WriteLine($"Price change for {product.Id}: {product.Price} → {request.Price}");

            product.Price = request.Price;
            product.LastUpdatedAt = DateTime.UtcNow;

            return NoContent();
        }

        // 7️ POST /api/products/{id}/image
        [HttpPost("{id:guid}/image")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public IActionResult UploadImage(Guid id)
        {
            var file = Request.Form.Files.FirstOrDefault();

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

        // 8️ DELETE /api/products/{id}
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

        // 9️ GET /api/products/server-time
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

            var formattedDate = DateTime.Now.ToString("F", culture);

            return Ok(formattedDate);
        }
    }
}