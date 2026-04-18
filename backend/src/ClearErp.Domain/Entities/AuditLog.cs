using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class AuditLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public Guid PerformedByUserId { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    public string Details { get; set; } = string.Empty;

    public User? PerformedByUser { get; set; }

    public static AuditLog Create(string action, string entityName, Guid performedByUserId, string details, Guid? entityId = null)
    {
        Guard.AgainstNullOrWhiteSpace(action, nameof(action), 100);
        Guard.AgainstNullOrWhiteSpace(entityName, nameof(entityName), 100);
        Guard.AgainstEmpty(performedByUserId, nameof(performedByUserId));
        Guard.AgainstNullOrWhiteSpace(details, nameof(details), 4000);

        return new AuditLog
        {
            Action = action.Trim(),
            EntityName = entityName.Trim(),
            EntityId = entityId,
            PerformedByUserId = performedByUserId,
            PerformedAt = DateTime.UtcNow,
            Details = details.Trim()
        };
    }
}
