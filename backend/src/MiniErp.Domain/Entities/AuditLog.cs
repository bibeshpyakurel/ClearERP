using MiniErp.Domain.Common;

namespace MiniErp.Domain.Entities;

public sealed class AuditLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public Guid PerformedByUserId { get; set; }
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
    public string Details { get; set; } = string.Empty;

    public User? PerformedByUser { get; set; }
}
