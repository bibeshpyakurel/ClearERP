namespace ClearErp.Api.Contracts.Auth;

public sealed record CurrentUserResponse(
    Guid? UserId,
    string? Email,
    IReadOnlyCollection<string> Roles,
    bool IsAuthenticated);
