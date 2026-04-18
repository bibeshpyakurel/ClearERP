namespace ClearErp.Api.Contracts.Auth;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    Guid UserId,
    string Email,
    string FullName,
    IReadOnlyCollection<string> Roles);
