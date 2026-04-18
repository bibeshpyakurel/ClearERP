using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClearErp.Api.Contracts.Audit;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces.Repositories;
using ClearErp.Domain.Enums;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
[ApiExplorerSettings(GroupName = "Audit")]
[Produces("application/json")]
[Authorize(Roles = nameof(RoleName.Admin))]
public sealed class AuditLogsController(IAuditLogRepository auditLogRepository) : ControllerBase
{
    /// <summary>
    /// Returns the most recent audit log entries for administrator review.
    /// </summary>
    [ProducesResponseType(typeof(IReadOnlyList<AuditLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status403Forbidden)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuditLogResponse>>> GetRecent(CancellationToken cancellationToken)
    {
        var logs = await auditLogRepository.GetRecentAsync(100, cancellationToken);

        return Ok(logs.Select(log => new AuditLogResponse(
            log.Id,
            log.Action,
            log.EntityName,
            log.EntityId,
            log.PerformedByUserId,
            log.PerformedAt,
            log.Details)));
    }
}
