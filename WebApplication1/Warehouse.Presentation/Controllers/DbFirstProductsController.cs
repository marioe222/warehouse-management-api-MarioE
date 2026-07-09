using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/dbfirst/products")]
public class DbFirstProductsController : ControllerBase
{
    private readonly WarehouseDbFirstContext _context;

    public DbFirstProductsController(WarehouseDbFirstContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .Include(p => p.Supplier)
            .ToListAsync();

        return Ok(products);
    }
}