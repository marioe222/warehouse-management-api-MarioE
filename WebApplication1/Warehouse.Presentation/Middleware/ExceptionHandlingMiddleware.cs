using System.Net;
using System.Text.Json;
using FluentValidation;

using Warehouse.Application.Common.Exceptions;
using Warehouse.Domain.Exceptions;
using Warehouse.Presentation.Contracts;


namespace Warehouse.Presentation.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;


    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }



    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Unhandled exception occurred while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path
            );


            await HandleExceptionAsync(
                context,
                exception
            );
        }
    }



    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "INTERNAL_ERROR";
        var message = "An unexpected error occurred.";



        switch (exception)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorCode = "NOT_FOUND";
                message = exception.Message;
                break;



            case BusinessRuleException:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "BUSINESS_RULE_ERROR";
                message = exception.Message;
                break;



            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "VALIDATION_ERROR";

                message = string.Join(
                    ", ",
                    validationException.Errors
                        .Select(x => x.ErrorMessage)
                );

                break;
        }



        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";



        var response = new ApiErrorResponse(
            errorCode,
            message,
            context.TraceIdentifier
        );



        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response)
        );
    }
}