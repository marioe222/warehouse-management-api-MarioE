using Microsoft.AspNetCore.Mvc;
using warehousemanagement.Contracts;
using warehousemanagement.Data;
using warehousemanagement.modules;

namespace WarehouseManagement.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var suppliers = FakeWarehouseStore.Suppliers
                .Where(s => s.IsActive)
                .ToList();

            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var supplier = FakeWarehouseStore.Suppliers
                .FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateSupplierRequest request)
        {
            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Country = request.Country,
                ContactEmail = request.ContactEmail,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            FakeWarehouseStore.Suppliers.Add(supplier);

            return Ok(supplier);
        }

        [HttpDelete("{id}")]
        public IActionResult Deactivate(Guid id)
        {
            var supplier = FakeWarehouseStore.Suppliers
                .FirstOrDefault(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            supplier.IsActive = false;

            return NoContent();
        }
    }
}