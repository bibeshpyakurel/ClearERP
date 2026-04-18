namespace ClearErp.Infrastructure.Persistence.Seeding;

public static class SeedConstants
{
    public static readonly Guid AdminRoleId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid InventoryManagerRoleId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid WarehouseStaffRoleId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    public static readonly Guid ViewerRoleId = Guid.Parse("10000000-0000-0000-0000-000000000004");

    public static readonly Guid AdminUserId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid WarehouseUserId = Guid.Parse("20000000-0000-0000-0000-000000000002");

    public static readonly Guid SeatingCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid StorageCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid ComponentsCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000003");

    public static readonly Guid AcmeSupplierId = Guid.Parse("40000000-0000-0000-0000-000000000001");
    public static readonly Guid NorthwoodSupplierId = Guid.Parse("40000000-0000-0000-0000-000000000002");
    public static readonly Guid EastlakeSupplierId = Guid.Parse("40000000-0000-0000-0000-000000000003");

    public static readonly Guid MainWarehouseId = Guid.Parse("50000000-0000-0000-0000-000000000001");
    public static readonly Guid MainAisleLocationId = Guid.Parse("60000000-0000-0000-0000-000000000001");

    public static readonly Guid TaskChairItemId = Guid.Parse("70000000-0000-0000-0000-000000000001");
    public static readonly Guid FilingCabinetItemId = Guid.Parse("70000000-0000-0000-0000-000000000002");
    public static readonly Guid DeskLegKitItemId = Guid.Parse("70000000-0000-0000-0000-000000000003");
    public static readonly Guid LoungeChairItemId = Guid.Parse("70000000-0000-0000-0000-000000000004");
    public static readonly Guid MobilePedestalItemId = Guid.Parse("70000000-0000-0000-0000-000000000005");
    public static readonly Guid ConferenceTableBaseItemId = Guid.Parse("70000000-0000-0000-0000-000000000006");

    public static readonly Guid AcmeTaskChairSupplierItemId = Guid.Parse("71000000-0000-0000-0000-000000000001");
    public static readonly Guid AcmeCabinetSupplierItemId = Guid.Parse("71000000-0000-0000-0000-000000000002");
    public static readonly Guid NorthwoodDeskLegSupplierItemId = Guid.Parse("71000000-0000-0000-0000-000000000003");
    public static readonly Guid EastlakeLoungeChairSupplierItemId = Guid.Parse("71000000-0000-0000-0000-000000000004");
    public static readonly Guid EastlakePedestalSupplierItemId = Guid.Parse("71000000-0000-0000-0000-000000000005");
    public static readonly Guid NorthwoodConferenceBaseSupplierItemId = Guid.Parse("71000000-0000-0000-0000-000000000006");

    public static readonly Guid DraftPurchaseOrderId = Guid.Parse("90000000-0000-0000-0000-000000000001");
    public static readonly Guid DraftPurchaseOrderLineId = Guid.Parse("90000000-0000-0000-0000-000000000002");
    public static readonly Guid ApprovedPurchaseOrderId = Guid.Parse("90000000-0000-0000-0000-000000000003");
    public static readonly Guid ApprovedPurchaseOrderLineId = Guid.Parse("90000000-0000-0000-0000-000000000004");
    public static readonly Guid PartialPurchaseOrderId = Guid.Parse("90000000-0000-0000-0000-000000000005");
    public static readonly Guid PartialPurchaseOrderLineId = Guid.Parse("90000000-0000-0000-0000-000000000006");
    public static readonly Guid CompletedPurchaseOrderId = Guid.Parse("90000000-0000-0000-0000-000000000007");
    public static readonly Guid CompletedPurchaseOrderLineId = Guid.Parse("90000000-0000-0000-0000-000000000008");
    public static readonly Guid CancelledPurchaseOrderId = Guid.Parse("90000000-0000-0000-0000-000000000009");
    public static readonly Guid CancelledPurchaseOrderLineId = Guid.Parse("90000000-0000-0000-0000-000000000010");
    public static readonly Guid EastlakeApprovedPurchaseOrderId = Guid.Parse("90000000-0000-0000-0000-000000000011");
    public static readonly Guid EastlakeApprovedPurchaseOrderLineId = Guid.Parse("90000000-0000-0000-0000-000000000012");
    public static readonly Guid DraftPurchaseOrderSecondLineId = Guid.Parse("90000000-0000-0000-0000-000000000013");
    public static readonly Guid ApprovedPurchaseOrderSecondLineId = Guid.Parse("90000000-0000-0000-0000-000000000014");

    public static readonly Guid PartialGoodsReceiptId = Guid.Parse("91000000-0000-0000-0000-000000000001");
    public static readonly Guid PartialGoodsReceiptLineId = Guid.Parse("91000000-0000-0000-0000-000000000002");
    public static readonly Guid CompletedGoodsReceiptId = Guid.Parse("91000000-0000-0000-0000-000000000003");
    public static readonly Guid CompletedGoodsReceiptLineId = Guid.Parse("91000000-0000-0000-0000-000000000004");

    public static readonly Guid ReceiptTransactionId = Guid.Parse("92000000-0000-0000-0000-000000000001");
    public static readonly Guid IssueTransactionId = Guid.Parse("92000000-0000-0000-0000-000000000002");
    public static readonly Guid AdjustmentIncreaseTransactionId = Guid.Parse("92000000-0000-0000-0000-000000000003");
    public static readonly Guid AdjustmentDecreaseTransactionId = Guid.Parse("92000000-0000-0000-0000-000000000004");
    public static readonly Guid PedestalReceiptTransactionId = Guid.Parse("92000000-0000-0000-0000-000000000005");
    public static readonly Guid TaskChairIssueTransactionId = Guid.Parse("92000000-0000-0000-0000-000000000006");
    public static readonly Guid ConferenceBaseAdjustmentTransactionId = Guid.Parse("92000000-0000-0000-0000-000000000007");

    public static readonly Guid DeskLegCycleCountAdjustmentId = Guid.Parse("93000000-0000-0000-0000-000000000001");
    public static readonly Guid CabinetDamageAdjustmentId = Guid.Parse("93000000-0000-0000-0000-000000000002");
    public static readonly Guid ConferenceBaseAdjustmentId = Guid.Parse("93000000-0000-0000-0000-000000000003");

    public static readonly Guid PurchaseOrderCreatedAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000001");
    public static readonly Guid GoodsReceiptAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000002");
    public static readonly Guid StockIssueAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000003");
    public static readonly Guid StockAdjustmentAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000004");
    public static readonly Guid EastlakePurchaseOrderAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000005");
    public static readonly Guid PedestalReceiptAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000006");
    public static readonly Guid TaskChairIssueAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000007");
    public static readonly Guid ConferenceBaseAdjustmentAuditLogId = Guid.Parse("94000000-0000-0000-0000-000000000008");
}
