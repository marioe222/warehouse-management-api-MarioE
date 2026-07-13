using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Dashboard.Queries.GetInventoryDashboard;


namespace Warehouse.Presentation.Controllers;


[ApiController]
[Route("api/inventory")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;


    public DashboardController(
        IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(
        CancellationToken cancellationToken)
    {
        var result =
            await _mediator.Send(
                new GetInventoryDashboardQuery(),
                cancellationToken
            );


        return Ok(result);
    }
}