using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using Warehouse.Application.Suppliers.Commands.CreateSupplier;
using Warehouse.Application.Suppliers.Commands.DeactivateSupplier;

using Warehouse.Application.Suppliers.Queries.GetSupplierById;
using Warehouse.Application.Suppliers.Queries.ListSuppliers;

using Warehouse.Application.ViewModels;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public SuppliersController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }



    // GET /api/suppliers
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new ListSuppliersQuery(),
            cancellationToken
        );


        var result = _mapper.Map<List<SupplierViewModel>>(
            response.Suppliers
        );


        return Ok(result);
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
            return NotFound();


        var result = _mapper.Map<SupplierViewModel>(
            supplier
        );


        return Ok(result);
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


        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            _mapper.Map<SupplierViewModel>(response)
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
            return NotFound();


        return NoContent();
    }
}