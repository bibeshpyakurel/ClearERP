using System.Text.Json;
using FluentValidation;
using ClearErp.Domain.Common;

namespace ClearErp.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static readonly Dictionary<int, string> ProblemTypeUris = new()
    {
        [StatusCodes.Status400BadRequest]          = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        [StatusCodes.Status401Unauthorized]        = "https://tools.ietf.org/html/rfc9110#section-15.5.2",
        [StatusCodes.Status403Forbidden]           = "https://tools.ietf.org/html/rfc9110#section-15.5.4",
        [StatusCodes.Status404NotFound]            = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        [StatusCodes.Status409Conflict]            = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        [StatusCodes.Status500InternalServerError] = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
    };

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception while processing request {Path}", context.Request.Path);

        var (statusCode, title, detail, errors) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation failed",
                "One or more validation errors occurred.",
                validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage).Distinct().ToArray())
            ),
            UnauthorizedException unauthorizedException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                unauthorizedException.Message,
                (IDictionary<string, string[]>?)null
            ),
            NotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFoundException.Message,
                (IDictionary<string, string[]>?)null
            ),
            ConflictException conflictException => (
                StatusCodes.Status409Conflict,
                "Conflict",
                conflictException.Message,
                (IDictionary<string, string[]>?)null
            ),
            DomainException domainException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                domainException.Message,
                (IDictionary<string, string[]>?)null
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred.",
                (IDictionary<string, string[]>?)null
            )
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var type = ProblemTypeUris.GetValueOrDefault(statusCode, "https://tools.ietf.org/html/rfc9110");
        var instance = context.Request.Path.Value;

        var response = new ApiErrorResponse(
            statusCode,
            title,
            detail,
            context.TraceIdentifier,
            type,
            instance,
            errors);

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase }));
    }
}
