using System.Diagnostics;

namespace Warehouse.Presentation.Middleware;

public class RequestTimingMiddleware
{
    private readonly ILogger<RequestTimingMiddleware> _logger;
    private readonly RequestDelegate _next;


    public RequestTimingMiddleware(
        RequestDelegate next,
        ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task InvokeAsync(
        HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();


        await _next(context);


        stopwatch.Stop();


        _logger.LogInformation(
            "Request {Method} {Path} completed in {ElapsedMilliseconds} ms",
            context.Request.Method,
            context.Request.Path,
            stopwatch.ElapsedMilliseconds
        );
    }
}