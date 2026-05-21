using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using MunicipiosIbge.Api.Common.Exceptions;
using MunicipiosIbge.Api.Common.Middleware;

namespace MunicipiosIbge.Tests.Common.Middleware;

public sealed class ExceptionHandlingMiddlewareTests
{
    [Theory]
    [InlineData(typeof(NotFoundException), StatusCodes.Status404NotFound)]
    [InlineData(typeof(ExternalServiceException), StatusCodes.Status502BadGateway)]
    [InlineData(typeof(InvalidOperationException), StatusCodes.Status400BadRequest)]
    [InlineData(typeof(Exception), StatusCodes.Status500InternalServerError)]
    public async Task InvokeAsync_WhenExceptionIsThrown_WritesExpectedStatusCode(
        Type exceptionType,
        int expectedStatusCode)
    {
        var exception = CreateException(exceptionType);
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
        var middleware = new ExceptionHandlingMiddleware(
            _ => throw exception,
            NullLogger<ExceptionHandlingMiddleware>.Instance);

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        using var payload = await JsonDocument.ParseAsync(context.Response.Body);

        Assert.Equal(expectedStatusCode, context.Response.StatusCode);
        Assert.StartsWith("application/json", context.Response.ContentType);
        Assert.False(payload.RootElement.GetProperty("success").GetBoolean());
    }

    private static Exception CreateException(Type exceptionType)
    {
        if (exceptionType == typeof(NotFoundException))
        {
            return new NotFoundException("not found");
        }

        if (exceptionType == typeof(ExternalServiceException))
        {
            return new ExternalServiceException("external");
        }

        if (exceptionType == typeof(InvalidOperationException))
        {
            return new InvalidOperationException("invalid");
        }

        return new Exception("unexpected");
    }
}
