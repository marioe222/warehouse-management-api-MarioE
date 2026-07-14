using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using Warehouse.Application.Products.Commands.ArchiveProduct;
using Warehouse.Application.Products.Commands.AssignSupplier;
using Warehouse.Application.Products.Commands.CreateProduct;
using Warehouse.Application.Products.Commands.UpdateProductPrice;
using Warehouse.Application.Products.Commands.UpdateProductQuantity;
using Warehouse.Application.Products.Commands.UploadProductImage;

using Warehouse.Application.Products.Queries.GetProductById;
using Warehouse.Application.Products.Queries.ListProducts;
using Warehouse.Application.Products.Queries.SearchProducts;

using Warehouse.Application.ViewModels;
using Warehouse.Presentation.Contracts;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public ProductsController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool onlyAvailable = false)
    {
        var response = await _mediator.Send(
            new ListProductsQuery(onlyAvailable)
        );


        var result = _mapper.Map<List<ProductViewModel>>(
            response.Products
        );


        return Ok(result);
    }



    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _mediator.Send(
            new GetProductByIdQuery(id)
        );


        if (product == null)
            return NotFound();


        var result = _mapper.Map<ProductViewModel>(product);


        return Ok(result);
    }



    // GET: api/products/search
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


        return Ok(
            _mapper.Map<List<ProductViewModel>>(products)
        );
    }



    // POST: api/products
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
                request.ExpiryDate
            )
        );


        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            _mapper.Map<ProductViewModel>(response)
        );
    }



    // POST: api/products/{id}/quantity
    [HttpPost("{id:guid}/quantity")]
    public async Task<IActionResult> UpdateQuantity(
        Guid id,
        UpdateProductQuantityRequest request)
    {
        if (request.QuantityInStock < 0)
            return BadRequest(
                "Quantity cannot be negative."
            );


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



    // POST: api/products/{id}/price
    [HttpPost("{id:guid}/price")]
    public async Task<IActionResult> UpdatePrice(
        Guid id,
        UpdateProductPriceRequest request)
    {
        if (request.Price <= 0)
            return BadRequest(
                "Price must be greater than zero."
            );


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



    // POST: api/products/{id}/image
    [HttpPost("{id:guid}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
        Guid id,
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



    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(
            new ArchiveProductCommand(id)
        );


        if (!result.Success)
            return NotFound();


        return NoContent();
    }



    // GET: api/products/server-time
    [HttpGet("server-time")]
    public IActionResult GetServerTime(
        [FromHeader(Name = "Accept-Language")] string language)
    {
        var time = DateTime.UtcNow;

        return Ok(time);
    }



    // POST: api/products/{id}/assign-supplier/{supplierId}
    [HttpPost("{id:guid}/assign-supplier/{supplierId:guid}")]
    public async Task<IActionResult> AssignSupplier(
        Guid id,
        Guid supplierId)
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