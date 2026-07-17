using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Application.Suppliers.Commands.DeactivateSupplier;
using Warehouse.Application.Suppliers.Queries.GetSupplierById;
using Warehouse.Application.Suppliers.Queries.ListSuppliers;
using Warehouse.Application.ViewModels;
using Warehouse.Presentation.Resources;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ILogger<SuppliersController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;


    public SuppliersController(
        IMediator mediator,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer,
        ILogger<SuppliersController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _localizer = localizer;
        _logger = logger;
    }


    // GET /api/suppliers
    [HttpGet]
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
}