using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/stock-adjustments")]
public class StockAdjustmentsController : ControllerBase
{
    private readonly IMediator _mediator;


    public StockAdjustmentsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }


    // POST: api/stock-adjustments
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateStockAdjustmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken
        );


        return CreatedAtAction(
            nameof(Create),
            result
        );
    }
}