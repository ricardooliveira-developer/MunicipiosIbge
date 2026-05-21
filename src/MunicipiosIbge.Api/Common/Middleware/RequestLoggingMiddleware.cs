using System.Diagnostics;

namespace MunicipiosIbge.Api.Common.Middleware;

public sealed class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        using var scope = logger.BeginScope(new Dictionary<string, object?>
        {
            ["TraceId"] = context.TraceIdentifier,
            ["Method"] = context.Request.Method,
            ["Path"] = context.Request.Path.Value
        });

        logger.LogInformation(
            "Requisição recebida: {Method} {Path}.",
            context.Request.Method,
            context.Request.Path);

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            logger.LogInformation(
                "Requisição finalizada: {Method} {Path} respondeu {StatusCode} em {ElapsedMilliseconds}ms.",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}
