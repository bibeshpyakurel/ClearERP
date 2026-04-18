namespace ClearErp.Api.Contracts.Audit;

public sealed record AuditLogResponse(
    Guid Id,
    string Action,
    string EntityName,
    Guid? EntityId,
    Guid PerformedByUserId,
    DateTime PerformedAt,
    string Details);
