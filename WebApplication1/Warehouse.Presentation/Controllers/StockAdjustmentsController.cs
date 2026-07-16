using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;
using Warehouse.Presentation.Resources;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/stock-adjustments")]
public class StockAdjustmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;


    public StockAdjustmentsController(
        IMediator mediator,
        IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
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
            new
            {
                message = _localizer["StockAdjustmentCreated"],
                data = result
            }
        );
    }
}