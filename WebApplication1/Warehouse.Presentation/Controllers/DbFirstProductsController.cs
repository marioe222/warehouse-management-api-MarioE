using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Models;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/dbfirst/products")]
public class DbFirstProductsController(WarehouseDbFirstContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await context.Products
            .ToListAsync();

        return Ok(products);
    }
}