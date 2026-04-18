using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.Auth;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application.Common.Interfaces.Services;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Auth")]
[Produces("application/json")]
public sealed class AuthController(IAuthService authService, ICurrentUserService currentUserService) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and returns a JWT access token for calling protected ERP endpoints.
    /// </summary>
    /// <remarks>
    /// Demo flow:
    /// 1. Call this endpoint with one of the seeded users.
    /// 2. Copy the returned access token.
    /// 3. Click Authorize in Swagger and paste the token as `Bearer {token}`.
    /// </remarks>
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request.Email, request.Password, cancellationToken);

        return Ok(new AuthResponse(
            result.AccessToken,
            result.ExpiresAtUtc,
            result.UserId,
            result.Email,
            result.FullName,
            result.Roles));
    }

    /// <summary>
    /// Returns the current authenticated user's identity and roles from the JWT context.
    /// </summary>
    [Authorize]
    [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [HttpGet("me")]
    public ActionResult<CurrentUserResponse> Me()
    {
        return Ok(new CurrentUserResponse(
            currentUserService.UserId,
            currentUserService.Email,
            currentUserService.Roles,
            currentUserService.IsAuthenticated));
    }
}
