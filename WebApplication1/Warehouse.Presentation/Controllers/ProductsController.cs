using MediatR;
using Microsoft.AspNetCore.Mvc;

using Warehouse.Application.Products.Commands.ArchiveProduct;
using Warehouse.Application.Products.Commands.AssignSupplier;
using Warehouse.Application.Products.Commands.CreateProduct;
using Warehouse.Application.Products.Commands.UpdateProductPrice;
using Warehouse.Application.Products.Commands.UpdateProductQuantity;
using Warehouse.Application.Products.Commands.UploadProductImage;

using Warehouse.Application.Products.Queries.GetProductById;
using Warehouse.Application.Products.Queries.ListProducts;
using Warehouse.Application.Products.Queries.SearchProducts;

using Warehouse.Presentation.Contracts;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;


    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    // 1 GET /api/products
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool onlyAvailable = false)
    {
        var products = await _mediator.Send(
            new ListProductsQuery(onlyAvailable)
        );

        return Ok(products);
    }



    // 2 GET /api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _mediator.Send(
            new GetProductByIdQuery(id)
        );


        if (product == null)
            return NotFound();


        return Ok(product);
    }



    // 3 SEARCH
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? supplier)
    {
        var products = await _mediator.Send(
            new SearchProductsQuery(
                name,
                supplier
            )
        );


        return Ok(products);
    }



    // 4 CREATE
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateProductRequest request)
    {
        var response = await _mediator.Send(
            new CreateProductCommand(
                request.Name,
                request.Sku,
                request.Description,
                request.Price,
                request.QuantityInStock,
                request.SupplierName,
                request.SupplierId,
                request.ExpiryDate
            )
        );


        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response
        );
    }



    // 5 UPDATE QUANTITY
    [HttpPost("{id:int}/quantity")]
    public async Task<IActionResult> UpdateQuantity(
        int id,
        UpdateProductQuantityRequest request)
    {
        if (request.QuantityInStock < 0)
            return BadRequest("Quantity cannot be negative.");


        var result = await _mediator.Send(
            new UpdateProductQuantityCommand(
                id,
                request.QuantityInStock
            )
        );


        if (!result.Success)
            return NotFound();


        return NoContent();
    }



    // 6 UPDATE PRICE
    [HttpPost("{id:int}/price")]
    public async Task<IActionResult> UpdatePrice(
        int id,
        UpdateProductPriceRequest request)
    {
        if (request.Price <= 0)
            return BadRequest("Price must be greater than zero.");


        var result = await _mediator.Send(
            new UpdateProductPriceCommand(
                id,
                request.Price
            )
        );


        if (!result.Success)
            return NotFound();


        return NoContent();
    }



    // 7 IMAGE UPLOAD
    [HttpPost("{id:int}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
        int id,
        IFormFile file)
    {
        var result = await _mediator.Send(
            new UploadProductImageCommand(
                id,
                file
            )
        );


        if (!result.Success)
            return NotFound();


        return Ok();
    }



    // 8 DELETE / ARCHIVE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(
            new ArchiveProductCommand(id)
        );


        if (!result.Success)
            return NotFound();


        return NoContent();
    }



    // 9 SERVER TIME
    [HttpGet("server-time")]
    public IActionResult GetServerTime(
        [FromHeader(Name = "Accept-Language")] string language)
    {
        var time = DateTime.UtcNow;

        return Ok(time);
    }



    // 10 ASSIGN SUPPLIER
    [HttpPost("{id:int}/assign-supplier/{supplierId:int}")]
    public async Task<IActionResult> AssignSupplier(
        int id,
        int supplierId)
    {
        var result = await _mediator.Send(
            new AssignSupplierCommand(
                id,
                supplierId
            )
        );


        if (!result.Success)
            return NotFound();


        return NoContent();
    }
}