using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Warehouse.Application.Dashboard.Queries.GetInventoryDashboard;
using Warehouse.Presentation.Resources;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/inventory")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly ILogger<DashboardController> _logger;


    public DashboardController(
        IMediator mediator,
        IStringLocalizer<SharedResources> localizer,
        ILogger<DashboardController> logger)
    {
        _mediator = mediator;
        _localizer = localizer;
        _logger = logger;
    }



    // GET: api/inventory/dashboard
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Inventory dashboard request started");


        var result = await _mediator.Send(
            new GetInventoryDashboardQuery(),
            cancellationToken
        );


        _logger.LogInformation(
            "Inventory dashboard retrieved successfully");


        return Ok(new
        {
            message = _localizer["DashboardRetrieved"].Value,
            data = result
        });
    }
}