using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<Item> Items { get; set; } = new List<Item>();

    public static Category Create(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 100);

        return new Category
        {
            Name = name.Trim()
        };
    }

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 100);
        Name = name.Trim();
        Touch();
    }
}
