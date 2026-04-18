using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public static Role Create(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 50);

        return new Role
        {
            Name = name.Trim()
        };
    }
}
