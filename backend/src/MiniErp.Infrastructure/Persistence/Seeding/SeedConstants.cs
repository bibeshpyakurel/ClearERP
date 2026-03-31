namespace MiniErp.Infrastructure.Persistence.Seeding;

public static class SeedConstants
{
    public static readonly Guid AdminRoleId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid InventoryManagerRoleId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid WarehouseStaffRoleId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    public static readonly Guid ViewerRoleId = Guid.Parse("10000000-0000-0000-0000-000000000004");

    public static readonly Guid AdminUserId = Guid.Parse("20000000-0000-0000-0000-000000000001");

    public static readonly Guid SeatingCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid StorageCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid ComponentsCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000003");

    public static readonly Guid AcmeSupplierId = Guid.Parse("40000000-0000-0000-0000-000000000001");
    public static readonly Guid NorthwoodSupplierId = Guid.Parse("40000000-0000-0000-0000-000000000002");

    public static readonly Guid MainWarehouseId = Guid.Parse("50000000-0000-0000-0000-000000000001");
    public static readonly Guid MainAisleLocationId = Guid.Parse("60000000-0000-0000-0000-000000000001");

    public static readonly Guid TaskChairItemId = Guid.Parse("70000000-0000-0000-0000-000000000001");
    public static readonly Guid FilingCabinetItemId = Guid.Parse("70000000-0000-0000-0000-000000000002");
    public static readonly Guid DeskLegKitItemId = Guid.Parse("70000000-0000-0000-0000-000000000003");
}
