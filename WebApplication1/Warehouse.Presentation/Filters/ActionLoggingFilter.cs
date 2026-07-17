using Microsoft.AspNetCore.Mvc.Filters;

namespace Warehouse.Presentation.Filters;

public class ActionLoggingFilter : IActionFilter
{
    private readonly ILogger<ActionLoggingFilter> _logger;


    public ActionLoggingFilter(
        ILogger<ActionLoggingFilter> logger)
    {
        _logger = logger;
    }


    public void OnActionExecuting(
        ActionExecutingContext context)
    {
        _logger.LogInformation(
            "Executing {Action}",
            context.ActionDescriptor.DisplayName
        );
    }


    public void OnActionExecuted(
        ActionExecutedContext context)
    {
        _logger.LogInformation(
            "Finished {Action}",
            context.ActionDescriptor.DisplayName
        );
    }
}