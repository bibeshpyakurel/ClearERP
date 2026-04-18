using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public User? User { get; set; }
    public Role? Role { get; set; }

    public static UserRole Create(Guid userId, Guid roleId)
    {
        Guard.AgainstEmpty(userId, nameof(userId));
        Guard.AgainstEmpty(roleId, nameof(roleId));

        return new UserRole
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}
