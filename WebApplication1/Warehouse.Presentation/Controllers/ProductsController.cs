using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
using Warehouse.Presentation.Resources;
using Warehouse.Application.Interfaces;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IStorageService _storageService;


    public ProductsController(
        IMediator mediator,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer,
        ILogger<ProductsController> logger,
        IStorageService storageService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _localizer = localizer;
        _logger = logger;
        _storageService = storageService;
    }


    // GET: api/products
    [HttpGet]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool onlyAvailable = false,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new ListProductsQuery(onlyAvailable),
            cancellationToken
        );


        var result = _mapper.Map<List<ProductViewModel>>(
            response.Products
        );


        _logger.LogInformation(
            "Retrieved {ProductCount} products",
            result.Count);


        return Ok(result);
    }


    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetProductByIdQuery(id),
            cancellationToken
        );


        if (response == null)
        {
            _logger.LogWarning(
                "Product {ProductId} not found",
                id);

            return NotFound(new
            {
                message = _localizer["ProductNotFound"].Value
            });
        }


        _logger.LogInformation(
            "Retrieved product {ProductId}",
            id);


        return Ok(response);
    }


    // GET: api/products/search
    [HttpGet("search")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? supplier,
        CancellationToken cancellationToken = default)
    {
        var products = await _mediator.Send(
            new SearchProductsQuery(
                name,
                supplier
            ),
            cancellationToken
        );


        _logger.LogInformation(
            "Product search executed. Name: {Name}, Supplier: {Supplier}",
            name,
            supplier);


        return Ok(
            _mapper.Map<List<ProductViewModel>>(products)
        );
    }


    // POST: api/products
    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
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
            ),
            cancellationToken
        );


        _logger.LogInformation(
            "Product {ProductId} created",
            response.Id);


        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            new
            {
                message = _localizer["ProductCreated"].Value,
                data = _mapper.Map<ProductViewModel>(response)
            }
        );
    }


    // POST: api/products/{id}/quantity
    [HttpPost("{id:guid}/quantity")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateQuantity(
        Guid id,
        UpdateProductQuantityRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new UpdateProductQuantityCommand(
                id,
                request.QuantityInStock
            ),
            cancellationToken
        );


        if (!result.Success)
        {
            return NotFound(new
            {
                message = _localizer["ProductNotFound"].Value
            });
        }


        return Ok(new
        {
            message = _localizer["ProductUpdated"].Value
        });
    }


    // POST: api/products/{id}/price
    [HttpPost("{id:guid}/price")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdatePrice(
        Guid id,
        UpdateProductPriceRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new UpdateProductPriceCommand(
                id,
                request.Price
            ),
            cancellationToken
        );


        if (!result.Success)
        {
            return NotFound(new
            {
                message = _localizer["ProductNotFound"].Value
            });
        }


        return Ok(new
        {
            message = _localizer["ProductUpdated"].Value
        });
    }


    // POST: api/products/{id}/image
    [HttpPost("{id:guid}/image")]
    [Authorize(Policy = "AdminPolicy")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
        Guid id,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new UploadProductImageCommand(
                id,
                file
            ),
            cancellationToken
        );


        if (!result.Success)
        {
            return NotFound(new
            {
                message = _localizer["ProductNotFound"].Value
            });
        }


        return Ok(new
        {
            message = _localizer["ProductUpdated"].Value
        });
    }


    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new ArchiveProductCommand(id),
            cancellationToken
        );


        if (!result.Success)
        {
            return NotFound(new
            {
                message = _localizer["ProductNotFound"].Value
            });
        }


        return Ok(new
        {
            message = _localizer["ProductDeleted"].Value
        });
    }


    // GET: api/products/server-time
    [HttpGet("server-time")]
    [Authorize(Policy = "UserPolicy")]
    public IActionResult GetServerTime(
        [FromHeader(Name = "Accept-Language")] string language)
    {
        return Ok(new
        {
            language,
            time = DateTime.UtcNow
        });
    }


    // POST: api/products/{id}/assign-supplier/{supplierId}
    [HttpPost("{id:guid}/assign-supplier/{supplierId:guid}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> AssignSupplier(
        Guid id,
        Guid supplierId,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new AssignSupplierCommand(
                id,
                supplierId
            ),
            cancellationToken
        );


        if (!result.Success)
        {
            return NotFound(new
            {
                message = _localizer["ProductNotFound"].Value
            });
        }


        return Ok(new
        {
            message = _localizer["ProductUpdated"].Value
        });
    }
    // GET: api/products/files/{objectKey}
    
    [HttpGet("files/{objectKey}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> DownloadFile(
        string objectKey,
        CancellationToken cancellationToken = default)
    {
        var stream = await _storageService.DownloadAsync(
            objectKey,
            cancellationToken
        );
    
    
        return File(
            stream,
            "application/octet-stream",
            objectKey
        );
    }
}