namespace ClearErp.Api.Contracts.Auth;

/// <summary>
/// Login request for retrieving a JWT access token.
/// Example payload:
/// {
///   "email": "admin@clearerp.local",
///   "password": "Admin123!"
/// }
/// </summary>
public sealed record LoginRequest(string Email, string Password);
