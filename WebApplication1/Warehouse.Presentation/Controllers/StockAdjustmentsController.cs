using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Warehouse.Application.StockAdjustments.Commands.CreateStockAdjustment;
using Warehouse.Presentation.Resources;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/stock-adjustments")]
public class StockAdjustmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ILogger<StockAdjustmentsController> _logger;


    public StockAdjustmentsController(
        IMediator mediator,
        IStringLocalizer<SharedResources> localizer,
        ILogger<StockAdjustmentsController> logger)
    {
        _mediator = mediator;
        _localizer = localizer;
        _logger = logger;
    }



    // POST: api/stock-adjustments
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateStockAdjustmentCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Stock adjustment creation started for Product {ProductId}",
            command.ProductId);


        var result = await _mediator.Send(
            command,
            cancellationToken
        );


        _logger.LogInformation(
            "Stock adjustment created successfully for Product {ProductId}",
            command.ProductId);



        return CreatedAtAction(
            nameof(Create),
            new
            {
                message = _localizer["StockAdjustmentCreated"].Value,
                data = result
            }
        );
    }
}