using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Application.Suppliers.Commands.DeactivateSupplier;
using Warehouse.Application.Suppliers.Commands.UploadSupplierDocument;
using Warehouse.Application.Suppliers.Queries.GetSupplierById;
using Warehouse.Application.Suppliers.Queries.ListSuppliers;
using Warehouse.Application.ViewModels;
using Warehouse.Presentation.Resources;
using Warehouse.Application.Interfaces;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ILogger<SuppliersController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IStorageService _storageService;


    public SuppliersController(
        IMediator mediator,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer,
        ILogger<SuppliersController> logger,
        IStorageService storageService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _localizer = localizer;
        _logger = logger;
        _storageService = storageService;
    }



    // GET /api/suppliers
    [HttpGet]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Retrieving all suppliers");


        var response = await _mediator.Send(
            new ListSuppliersQuery(),
            cancellationToken
        );


        var result = _mapper.Map<List<SupplierViewModel>>(
            response.Suppliers
        );


        _logger.LogInformation(
            "Retrieved {SupplierCount} suppliers",
            result.Count);


        return Ok(new
        {
            message = _localizer["SuppliersRetrieved"].Value,
            data = result
        });
    }




    // GET /api/suppliers/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var supplier = await _mediator.Send(
            new GetSupplierByIdQuery(id),
            cancellationToken
        );


        if (supplier == null)
        {
            _logger.LogWarning(
                "Supplier {SupplierId} not found",
                id);


            return NotFound(new
            {
                message = _localizer["SupplierNotFound"].Value
            });
        }


        var result = _mapper.Map<SupplierViewModel>(
            supplier
        );


        _logger.LogInformation(
            "Supplier {SupplierId} retrieved",
            id);


        return Ok(new
        {
            message = _localizer["SupplierRetrieved"].Value,
            data = result
        });
    }




    // POST /api/suppliers
    [HttpPost]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Create(
        CreateSupplierCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            command,
            cancellationToken
        );


        _logger.LogInformation(
            "Supplier {SupplierId} created",
            response.Id);


        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            new
            {
                message = _localizer["SupplierCreated"].Value,
                data = _mapper.Map<SupplierViewModel>(response)
            }
        );
    }




    // DELETE /api/suppliers/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new DeactivateSupplierCommand(id),
            cancellationToken
        );


        if (!result.Success)
        {
            _logger.LogWarning(
                "Failed deactivating supplier {SupplierId}",
                id);


            return NotFound(new
            {
                message = _localizer["SupplierNotFound"].Value
            });
        }


        _logger.LogInformation(
            "Supplier {SupplierId} deactivated",
            id);


        return Ok(new
        {
            message = _localizer["SupplierDeactivated"].Value
        });
    }




    // POST /api/suppliers/{id}/document
    [HttpPost("{id:guid}/document")]
    [Authorize(Policy = "AdminPolicy")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocument(
        Guid id,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {

        var result = await _mediator.Send(
            new UploadSupplierDocumentCommand(
                id,
                file
            ),
            cancellationToken
        );


        if (!result.Success)
        {
            _logger.LogWarning(
                "Failed uploading document for supplier {SupplierId}",
                id);


            return NotFound(new
            {
                message = _localizer["SupplierNotFound"].Value
            });
        }


        _logger.LogInformation(
            "Document uploaded successfully for supplier {SupplierId}",
            id);


        return Ok(new
        {
            message = "Supplier document uploaded successfully"
        });
    }
    
    // GET /api/suppliers/files/{objectKey}
    
    [HttpGet("files/{objectKey}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> DownloadDocument(
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