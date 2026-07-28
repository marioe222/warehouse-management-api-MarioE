using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Presentation.Middleware;

namespace Warehouse.Api.UnitTests.Middleware;

public class RequestTimingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldLogRequestPath()
    {
        var logger = new Mock<ILogger<RequestTimingMiddleware>>();

        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/api/products";


        var middleware = new RequestTimingMiddleware(
            next: ctx => Task.CompletedTask,
            logger.Object);

        
        await middleware.InvokeAsync(context);


        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, type) =>
                    state.ToString()!.Contains("/api/products")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }


    [Fact]
    public async Task InvokeAsync_ShouldLogResponseStatusCode()
    {
        var logger = new Mock<ILogger<RequestTimingMiddleware>>();

        var context = new DefaultHttpContext();


        var middleware = new RequestTimingMiddleware(
            next: ctx =>
            {
                ctx.Response.StatusCode = 201;
                return Task.CompletedTask;
            },
            logger.Object);


        await middleware.InvokeAsync(context);


        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, type) =>
                    state.ToString()!.Contains("201")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }


    [Fact]
    public async Task InvokeAsync_ShouldLogElapsedMilliseconds()
    {
        var logger = new Mock<ILogger<RequestTimingMiddleware>>();

        var context = new DefaultHttpContext();


        var middleware = new RequestTimingMiddleware(
            next: async ctx =>
            {
                await Task.Delay(50);
            },
            logger.Object);


        await middleware.InvokeAsync(context);


        logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, type) =>
                    state.ToString()!.Contains("ms")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}