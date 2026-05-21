using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Common.Middleware;

namespace MunicipiosIbge.Tests.Common.Middleware;

public sealed class RequestLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_CallsNextMiddleware()
    {
        var called = false;
        var context = new DefaultHttpContext();
        var middleware = new RequestLoggingMiddleware(
            nextContext =>
            {
                called = true;
                nextContext.Response.StatusCode = StatusCodes.Status204NoContent;
                return Task.CompletedTask;
            },
            NullLogger<RequestLoggingMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        Assert.True(called);
        Assert.Equal(StatusCodes.Status204NoContent, context.Response.StatusCode);
    }
}
