namespace ClearErp.Api.Middleware;

/// <summary>
/// RFC 7807 Problem Details error payload.
/// See: https://datatracker.ietf.org/doc/html/rfc7807
/// </summary>
public sealed record ApiErrorResponse(
    int Status,
    string Title,
    string Detail,
    string TraceId,
    string? Type = null,
    string? Instance = null,
    IDictionary<string, string[]>? Errors = null);
