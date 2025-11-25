using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Middlewares;

public sealed class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // ---- 1) Log the incoming request ----
        _logger.LogInformation(
            "Incoming HTTP {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        // ---- Continue the pipeline ----
        await _next(context);

        // ---- 2) Log the outgoing response ----
        _logger.LogInformation(
            "Outgoing HTTP {StatusCode} for {Method} {Path}",
            context.Response.StatusCode,
            context.Request.Method,
            context.Request.Path);
    }
}