using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Common;
using Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Api.Middlewares;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger)
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
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain error occurred.");

            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var result = Result.Fail(ex.Message);
            await context.Response.WriteAsJsonAsync(result);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage })
                .ToArray();

            var payload = new
            {
                Success = false,
                Error = "Validation failed.",
                Errors = errors
            };

            await context.Response.WriteAsJsonAsync(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var result = Result.Fail("Internal server error.");
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}