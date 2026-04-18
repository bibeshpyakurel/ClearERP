namespace ClearErp.Infrastructure.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "ClearErp";
    public string Audience { get; init; } = "ClearErp.Client";
    public string Key { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; } = 60;
}
