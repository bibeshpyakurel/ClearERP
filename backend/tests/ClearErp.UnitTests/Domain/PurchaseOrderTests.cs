using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;
using ClearErp.Domain.Enums;

namespace ClearErp.UnitTests.Domain;

public sealed class PurchaseOrderTests
{
    [Fact]
    public void Create_WithLines_Should_CalculateTotalAmount()
    {
        var purchaseOrder = PurchaseOrder.Create("PO-1001", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        purchaseOrder.AddLine(Guid.NewGuid(), 2, 50m);
        purchaseOrder.AddLine(Guid.NewGuid(), 3, 10m);

        Assert.Equal(130m, purchaseOrder.TotalAmount);
    }

    [Fact]
    public void Approve_Should_Throw_When_NoLinesExist()
    {
        var purchaseOrder = PurchaseOrder.Create("PO-1001", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        var exception = Assert.Throws<DomainException>(() => purchaseOrder.Approve());

        Assert.Equal("A purchase order must have at least one line before approval.", exception.Message);
    }

    [Fact]
    public void Approve_Should_SetStatusToApproved()
    {
        var purchaseOrder = PurchaseOrder.Create("PO-1001", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        purchaseOrder.AddLine(Guid.NewGuid(), 1, 20m);

        purchaseOrder.Approve();

        Assert.Equal(PurchaseOrderStatus.Approved, purchaseOrder.Status);
    }

    [Fact]
    public void ReplaceLines_Should_Throw_When_DuplicateItemsAreProvided()
    {
        var itemId = Guid.NewGuid();
        var purchaseOrder = PurchaseOrder.Create("PO-1001", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        var exception = Assert.Throws<DomainException>(() =>
            purchaseOrder.ReplaceLines(new[]
            {
                (itemId, 1, 10m),
                (itemId, 2, 12m)
            }));

        Assert.Equal("Each item may only appear once on a purchase order.", exception.Message);
    }
}
