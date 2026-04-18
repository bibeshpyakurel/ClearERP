namespace ClearErp.Application.Common.Interfaces.Services;

public sealed record AuthResult(
    string AccessToken,
    DateTime ExpiresAtUtc,
    Guid UserId,
    string Email,
    string FullName,
    IReadOnlyCollection<string> Roles);
