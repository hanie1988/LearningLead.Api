namespace Api.Middlewares;

using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 1) Read or generate correlation id
        var correlationId =
            context.Request.Headers.TryGetValue(HeaderName, out var existing)
                ? existing.ToString()
                : Guid.NewGuid().ToString();

        // 2) Put correlationId into response header
        context.Response.Headers[HeaderName] = correlationId;

        // 3) Attach to log scope so Serilog can include it
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();
            _logger.LogInformation(
                "Request {Method} {Path} finished with {StatusCode} in {Elapsed} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds);
        }
    }
}