using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using ClearErp.Api.Middleware;
using ClearErp.Domain.Common;

namespace ClearErp.UnitTests.Services;

public sealed class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task Invoke_WhenConflictExceptionIsThrown_ShouldReturnConflictResponse()
    {
        var context = CreateHttpContext();
        var middleware = new ExceptionHandlingMiddleware(
            _ => Task.FromException(new ConflictException("SKU must be unique.")),
            NullLogger<ExceptionHandlingMiddleware>.Instance);

        await middleware.Invoke(context);

        var payload = await ReadResponseAsync(context);

        Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(409, payload.Status);
        Assert.Equal("Conflict", payload.Title);
        Assert.Equal("SKU must be unique.", payload.Detail);
    }

    [Fact]
    public async Task Invoke_WhenValidationExceptionIsThrown_ShouldReturnValidationErrors()
    {
        var context = CreateHttpContext();
        var middleware = new ExceptionHandlingMiddleware(
            _ => Task.FromException(new ValidationException([
                new ValidationFailure("Email", "'Email' must not be empty."),
                new ValidationFailure("Password", "'Password' must be at least 6 characters.")
            ])),
            NullLogger<ExceptionHandlingMiddleware>.Instance);

        await middleware.Invoke(context);

        var payload = await ReadResponseAsync(context);

        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal("Validation failed", payload.Title);
        Assert.NotNull(payload.Errors);
        Assert.Contains("Email", payload.Errors.Keys);
        Assert.Contains("Password", payload.Errors.Keys);
    }

    [Fact]
    public async Task Invoke_WhenUnexpectedExceptionIsThrown_ShouldReturnInternalServerError()
    {
        var context = CreateHttpContext();
        var middleware = new ExceptionHandlingMiddleware(
            _ => Task.FromException(new InvalidOperationException("boom")),
            NullLogger<ExceptionHandlingMiddleware>.Instance);

        await middleware.Invoke(context);

        var payload = await ReadResponseAsync(context);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.NotNull(payload);
        Assert.Equal(500, payload.Status);
        Assert.Equal("Internal Server Error", payload.Title);
        Assert.Equal("An unexpected error occurred.", payload.Detail);
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        return new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream()
            }
        };
    }

    private static async Task<ApiErrorResponse?> ReadResponseAsync(HttpContext context)
    {
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var json = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<ApiErrorResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
