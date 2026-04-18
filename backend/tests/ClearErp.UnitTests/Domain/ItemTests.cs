using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;

namespace ClearErp.UnitTests.Domain;

public sealed class ItemTests
{
    [Fact]
    public void Create_Should_NormalizeSkuAndUnit()
    {
        var categoryId = Guid.NewGuid();

        var item = Item.Create(categoryId, " abc-123 ", "Desk", " ea ", 5, 99.99m, "demo");

        Assert.Equal("ABC-123", item.Sku);
        Assert.Equal("EA", item.Unit);
        Assert.Equal(categoryId, item.CategoryId);
    }

    [Fact]
    public void Create_Should_Throw_When_ReorderLevelIsNegative()
    {
        var action = () => Item.Create(Guid.NewGuid(), "SKU-1", "Desk", "EA", -1, 10m);

        var exception = Assert.Throws<DomainException>(action);
        Assert.Equal("reorderLevel must be zero or greater.", exception.Message);
    }

    [Fact]
    public void UpdateDetails_Should_UpdateCoreFields()
    {
        var item = Item.Create(Guid.NewGuid(), "SKU-1", "Desk", "EA", 2, 10m);
        var newCategoryId = Guid.NewGuid();

        item.UpdateDetails(newCategoryId, "SKU-2", "Chair", "set", 4, 25m, "updated");

        Assert.Equal(newCategoryId, item.CategoryId);
        Assert.Equal("SKU-2", item.Sku);
        Assert.Equal("Chair", item.Name);
        Assert.Equal("SET", item.Unit);
        Assert.Equal(4, item.ReorderLevel);
        Assert.Equal(25m, item.StandardCost);
        Assert.Equal("updated", item.Description);
        Assert.NotNull(item.UpdatedAt);
    }

    [Fact]
    public void ActivateDeactivate_Should_ToggleStatus()
    {
        var item = Item.Create(Guid.NewGuid(), "SKU-1", "Desk", "EA", 2, 10m);

        item.Deactivate();
        Assert.False(item.IsActive);

        item.Activate();
        Assert.True(item.IsActive);
    }
}
