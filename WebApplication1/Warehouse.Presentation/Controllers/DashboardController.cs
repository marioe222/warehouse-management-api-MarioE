using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using Warehouse.Application.Dashboard.Queries.GetInventoryDashboard;
using Warehouse.Presentation.Resources;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/inventory")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _localizer;


    public DashboardController(
        IMediator mediator,
        IStringLocalizer<SharedResources> localizer)
    {
        _mediator = mediator;
        _localizer = localizer;
    }



    // GET: api/inventory/dashboard
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetInventoryDashboardQuery(),
            cancellationToken
        );


        return Ok(new
        {
            message = _localizer["DashboardRetrieved"],
            data = result
        });
    }
}