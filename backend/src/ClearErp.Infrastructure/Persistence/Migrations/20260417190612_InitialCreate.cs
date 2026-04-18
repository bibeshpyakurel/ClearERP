using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClearErp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sku = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Unit = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ReorderLevel = table.Column<int>(type: "integer", nullable: false),
                    StandardCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_items_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    PerformedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Details = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_PerformedByUserId",
                        column: x => x.PerformedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PoNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_orders_suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_orders_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_user_roles_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_locations_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supplier_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierSku = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplier_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_supplier_items_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplier_items_suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "goods_receipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceivedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_goods_receipts_purchase_orders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "purchase_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goods_receipts_users_ReceivedByUserId",
                        column: x => x.ReceivedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "purchase_order_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderedQuantity = table.Column<int>(type: "integer", nullable: false),
                    ReceivedQuantity = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase_order_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchase_order_lines_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_purchase_order_lines_purchase_orders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "purchase_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_balances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityOnHand = table.Column<int>(type: "integer", nullable: false),
                    QuantityReserved = table.Column<int>(type: "integer", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_balances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_balances_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_balances_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_balances_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "inventory_transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    ReferenceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    QuantityChange = table.Column<int>(type: "integer", nullable: false),
                    BalanceAfter = table.Column<int>(type: "integer", nullable: false),
                    PerformedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_inventory_transactions_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_transactions_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_transactions_users_PerformedByUserId",
                        column: x => x.PerformedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inventory_transactions_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "stock_adjustments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdjustmentType = table.Column<int>(type: "integer", nullable: false),
                    QuantityDelta = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PerformedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_adjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_users_PerformedByUserId",
                        column: x => x.PerformedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_stock_adjustments_warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "goods_receipt_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GoodsReceiptId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedQuantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods_receipt_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_goods_receipt_lines_goods_receipts_GoodsReceiptId",
                        column: x => x.GoodsReceiptId,
                        principalTable: "goods_receipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_goods_receipt_lines_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_goods_receipt_lines_purchase_order_lines_PurchaseOrderLineId",
                        column: x => x.PurchaseOrderLineId,
                        principalTable: "purchase_order_lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Seating", null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Storage", null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Components", null }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "InventoryManager", null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "WarehouseStaff", null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Viewer", null }
                });

            migrationBuilder.InsertData(
                table: "suppliers",
                columns: new[] { "Id", "ContactName", "CreatedAt", "Email", "IsActive", "Name", "Notes", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), "Jordan Lee", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "orders@acme-industrial.example", true, "Acme Industrial Supply", null, "555-0100", null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "Taylor Smith", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sales@northwood.example", true, "Northwood Components", null, "555-0110", null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "Morgan Patel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "account-team@eastlake.example", true, "Eastlake Office Furnishings", null, "555-0120", null }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@clearerp.local", "System Administrator", true, "100000.D7/na2fAshvnIG4dYnDY3w==.r1Wxdl0Svjd+hM1EFn3gwvocm0IuZvGY0HTcOEAuGUM=", null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "warehouse@clearerp.local", "Warehouse Operator", true, "100000.yjJgxVnzj85lVCaN8of6NA==.NTw4Z2FymPSWFp5BzyaC8J1F9NpH6x8el2ST+FgofYE=", null }
                });

            migrationBuilder.InsertData(
                table: "warehouses",
                columns: new[] { "Id", "Code", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("50000000-0000-0000-0000-000000000001"), "MAIN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Main Warehouse", null });

            migrationBuilder.InsertData(
                table: "audit_logs",
                columns: new[] { "Id", "Action", "CreatedAt", "Details", "EntityId", "EntityName", "PerformedAt", "PerformedByUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("94000000-0000-0000-0000-000000000001"), "PurchaseOrderCreated", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Created approved demo purchase order PO-2026-002.", new Guid("90000000-0000-0000-0000-000000000003"), "PurchaseOrder", new DateTime(2026, 3, 22, 9, 30, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), null },
                    { new Guid("94000000-0000-0000-0000-000000000002"), "GoodsReceiptPosted", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Received 4 task chairs against PO-2026-003.", new Guid("91000000-0000-0000-0000-000000000001"), "GoodsReceipt", new DateTime(2026, 3, 25, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), null },
                    { new Guid("94000000-0000-0000-0000-000000000003"), "StockIssued", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Issued two filing cabinets for municipal office fit-out.", new Guid("92000000-0000-0000-0000-000000000002"), "InventoryTransaction", new DateTime(2026, 3, 29, 9, 15, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), null },
                    { new Guid("94000000-0000-0000-0000-000000000004"), "StockAdjusted", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Recorded a damaged filing cabinet after inbound inspection.", new Guid("93000000-0000-0000-0000-000000000002"), "StockAdjustment", new DateTime(2026, 3, 30, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), null },
                    { new Guid("94000000-0000-0000-0000-000000000005"), "PurchaseOrderCreated", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Created approved demo purchase order PO-2026-006 for Eastlake Office Furnishings.", new Guid("90000000-0000-0000-0000-000000000011"), "PurchaseOrder", new DateTime(2026, 3, 27, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), null },
                    { new Guid("94000000-0000-0000-0000-000000000006"), "GoodsReceiptPosted", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Recorded rush replenishment receipt for mobile pedestals.", new Guid("92000000-0000-0000-0000-000000000005"), "InventoryTransaction", new DateTime(2026, 3, 31, 9, 30, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), null },
                    { new Guid("94000000-0000-0000-0000-000000000007"), "StockIssued", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Issued two task chairs for showroom refresh.", new Guid("92000000-0000-0000-0000-000000000006"), "InventoryTransaction", new DateTime(2026, 3, 31, 13, 20, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), null },
                    { new Guid("94000000-0000-0000-0000-000000000008"), "StockAdjusted", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cycle count increased conference table base inventory by two units.", new Guid("93000000-0000-0000-0000-000000000003"), "StockAdjustment", new DateTime(2026, 3, 31, 15, 5, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), null }
                });

            migrationBuilder.InsertData(
                table: "items",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "IsActive", "Name", "ReorderLevel", "Sku", "StandardCost", "Unit", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Adjustable office task chair", true, "Task Chair", 10, "CHR-1001", 129.99m, "EA", null },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Three-drawer steel filing cabinet", true, "Filing Cabinet", 5, "CAB-2001", 249.00m, "EA", null },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Set of four metal desk legs", true, "Desk Leg Kit", 20, "KIT-3001", 58.50m, "SET", null },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Soft seating lounge chair for waiting areas", true, "Lounge Chair", 6, "LNG-4001", 219.00m, "EA", null },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lockable under-desk storage pedestal", true, "Mobile Pedestal", 8, "PED-5001", 179.00m, "EA", null },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Powder-coated steel base for conference tables", true, "Conference Table Base", 4, "BAS-6001", 84.00m, "EA", null }
                });

            migrationBuilder.InsertData(
                table: "locations",
                columns: new[] { "Id", "Code", "CreatedAt", "Name", "UpdatedAt", "WarehouseId" },
                values: new object[] { new Guid("60000000-0000-0000-0000-000000000001"), "A1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aisle A1", null, new Guid("50000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "purchase_orders",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "ExpectedDate", "OrderDate", "PoNumber", "Status", "SupplierId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "PO-2026-001", 1, new Guid("40000000-0000-0000-0000-000000000001"), null },
                    { new Guid("90000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Utc), "PO-2026-002", 2, new Guid("40000000-0000-0000-0000-000000000002"), null },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), "PO-2026-003", 3, new Guid("40000000-0000-0000-0000-000000000001"), null },
                    { new Guid("90000000-0000-0000-0000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "PO-2026-004", 4, new Guid("40000000-0000-0000-0000-000000000002"), null },
                    { new Guid("90000000-0000-0000-0000-000000000009"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), "PO-2026-005", 5, new Guid("40000000-0000-0000-0000-000000000001"), null },
                    { new Guid("90000000-0000-0000-0000-000000000011"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 27, 0, 0, 0, 0, DateTimeKind.Utc), "PO-2026-006", 2, new Guid("40000000-0000-0000-0000-000000000003"), null }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "goods_receipts",
                columns: new[] { "Id", "CreatedAt", "PurchaseOrderId", "ReceiptNumber", "ReceivedAt", "ReceivedByUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("91000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("90000000-0000-0000-0000-000000000005"), "GR-2026-003", new DateTime(2026, 3, 25, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), null },
                    { new Guid("91000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("90000000-0000-0000-0000-000000000007"), "GR-2026-004", new DateTime(2026, 3, 16, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), null }
                });

            migrationBuilder.InsertData(
                table: "inventory_balances",
                columns: new[] { "Id", "CreatedAt", "ItemId", "LocationId", "QuantityAvailable", "QuantityOnHand", "QuantityReserved", "UpdatedAt", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("80000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001"), 16, 18, 2, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000001"), 3, 4, 1, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), new Guid("60000000-0000-0000-0000-000000000001"), 28, 32, 4, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000004"), new Guid("60000000-0000-0000-0000-000000000001"), 2, 5, 3, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000005"), new Guid("60000000-0000-0000-0000-000000000001"), 12, 14, 2, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000006"), new Guid("60000000-0000-0000-0000-000000000001"), 10, 11, 1, null, new Guid("50000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "inventory_transactions",
                columns: new[] { "Id", "BalanceAfter", "CreatedAt", "ItemId", "LocationId", "PerformedAt", "PerformedByUserId", "QuantityChange", "Reason", "ReferenceId", "ReferenceType", "TransactionType", "UpdatedAt", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("92000000-0000-0000-0000-000000000001"), 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 25, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), 4, null, new Guid("91000000-0000-0000-0000-000000000001"), "GoodsReceipt", 1, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("92000000-0000-0000-0000-000000000002"), 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 29, 9, 15, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), -2, "Reserved for municipal office fit-out", null, "ManualIssue", 2, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("92000000-0000-0000-0000-000000000003"), 32, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 8, 45, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), 3, "Cycle count found extra kit components", new Guid("93000000-0000-0000-0000-000000000001"), "StockAdjustment", 3, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("92000000-0000-0000-0000-000000000004"), 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), -1, "One cabinet damaged in transit", new Guid("93000000-0000-0000-0000-000000000002"), "StockAdjustment", 4, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("92000000-0000-0000-0000-000000000005"), 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000005"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 31, 9, 30, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), 6, "Rush replenishment for mobile pedestals", new Guid("90000000-0000-0000-0000-000000000011"), "VendorDelivery", 1, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("92000000-0000-0000-0000-000000000006"), 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 31, 13, 20, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000002"), -2, "Moved chairs to showroom staging area", null, "ShowroomRefresh", 2, null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("92000000-0000-0000-0000-000000000007"), 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000006"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 31, 15, 5, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), 2, "Found two extra conference table bases during cycle count", new Guid("93000000-0000-0000-0000-000000000003"), "StockAdjustment", 3, null, new Guid("50000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "purchase_order_lines",
                columns: new[] { "Id", "CreatedAt", "ItemId", "OrderedQuantity", "PurchaseOrderId", "ReceivedQuantity", "UnitCost", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("90000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), 6, new Guid("90000000-0000-0000-0000-000000000001"), 0, 235.00m, null },
                    { new Guid("90000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), 12, new Guid("90000000-0000-0000-0000-000000000003"), 0, 54.75m, null },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), 10, new Guid("90000000-0000-0000-0000-000000000005"), 4, 121.50m, null },
                    { new Guid("90000000-0000-0000-0000-000000000008"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), 8, new Guid("90000000-0000-0000-0000-000000000007"), 8, 52.00m, null },
                    { new Guid("90000000-0000-0000-0000-000000000010"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), 3, new Guid("90000000-0000-0000-0000-000000000009"), 0, 125.00m, null },
                    { new Guid("90000000-0000-0000-0000-000000000012"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000005"), 10, new Guid("90000000-0000-0000-0000-000000000011"), 0, 176.50m, null },
                    { new Guid("90000000-0000-0000-0000-000000000013"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000004"), 4, new Guid("90000000-0000-0000-0000-000000000001"), 0, 210.00m, null },
                    { new Guid("90000000-0000-0000-0000-000000000014"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000006"), 6, new Guid("90000000-0000-0000-0000-000000000003"), 0, 84.00m, null }
                });

            migrationBuilder.InsertData(
                table: "stock_adjustments",
                columns: new[] { "Id", "AdjustmentType", "CreatedAt", "ItemId", "LocationId", "PerformedAt", "PerformedByUserId", "QuantityDelta", "Reason", "UpdatedAt", "WarehouseId" },
                values: new object[,]
                {
                    { new Guid("93000000-0000-0000-0000-000000000001"), 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 8, 45, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), 3, "Cycle count found extra kit components", null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("93000000-0000-0000-0000-000000000002"), 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 30, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), 1, "One cabinet damaged in transit", null, new Guid("50000000-0000-0000-0000-000000000001") },
                    { new Guid("93000000-0000-0000-0000-000000000003"), 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000006"), new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2026, 3, 31, 15, 5, 0, 0, DateTimeKind.Utc), new Guid("20000000-0000-0000-0000-000000000001"), 2, "Found two extra conference table bases during cycle count", null, new Guid("50000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "supplier_items",
                columns: new[] { "Id", "CreatedAt", "ItemId", "SupplierId", "SupplierSku", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("71000000-0000-0000-0000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), new Guid("40000000-0000-0000-0000-000000000001"), "ACME-CHR-1001", null },
                    { new Guid("71000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001"), "ACME-CAB-2001", null },
                    { new Guid("71000000-0000-0000-0000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000002"), "NW-KIT-3001", null },
                    { new Guid("71000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000003"), "EL-LNG-4001", null },
                    { new Guid("71000000-0000-0000-0000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000003"), "EL-PED-5001", null },
                    { new Guid("71000000-0000-0000-0000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000002"), "NW-BAS-6001", null }
                });

            migrationBuilder.InsertData(
                table: "goods_receipt_lines",
                columns: new[] { "Id", "CreatedAt", "GoodsReceiptId", "ItemId", "PurchaseOrderLineId", "ReceivedQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("91000000-0000-0000-0000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("91000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000001"), new Guid("90000000-0000-0000-0000-000000000006"), 4, null },
                    { new Guid("91000000-0000-0000-0000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("91000000-0000-0000-0000-000000000003"), new Guid("70000000-0000-0000-0000-000000000003"), new Guid("90000000-0000-0000-0000-000000000008"), 8, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_PerformedByUserId",
                table: "audit_logs",
                column: "PerformedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_Name",
                table: "categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipt_lines_GoodsReceiptId",
                table: "goods_receipt_lines",
                column: "GoodsReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipt_lines_ItemId",
                table: "goods_receipt_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipt_lines_PurchaseOrderLineId",
                table: "goods_receipt_lines",
                column: "PurchaseOrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipts_PurchaseOrderId",
                table: "goods_receipts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipts_ReceiptNumber",
                table: "goods_receipts",
                column: "ReceiptNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_goods_receipts_ReceivedByUserId",
                table: "goods_receipts",
                column: "ReceivedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_balances_ItemId_WarehouseId_LocationId",
                table: "inventory_balances",
                columns: new[] { "ItemId", "WarehouseId", "LocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_balances_LocationId",
                table: "inventory_balances",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_balances_WarehouseId",
                table: "inventory_balances",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_transactions_ItemId",
                table: "inventory_transactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_transactions_LocationId",
                table: "inventory_transactions",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_transactions_PerformedByUserId",
                table: "inventory_transactions",
                column: "PerformedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_transactions_WarehouseId",
                table: "inventory_transactions",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_items_CategoryId",
                table: "items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_items_Sku",
                table: "items",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_locations_WarehouseId_Code",
                table: "locations",
                columns: new[] { "WarehouseId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_lines_ItemId",
                table: "purchase_order_lines",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_order_lines_PurchaseOrderId",
                table: "purchase_order_lines",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_orders_CreatedByUserId",
                table: "purchase_orders",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_purchase_orders_PoNumber",
                table: "purchase_orders",
                column: "PoNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_purchase_orders_SupplierId",
                table: "purchase_orders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_roles_Name",
                table: "roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_ItemId",
                table: "stock_adjustments",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_LocationId",
                table: "stock_adjustments",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_PerformedByUserId",
                table: "stock_adjustments",
                column: "PerformedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_stock_adjustments_WarehouseId",
                table: "stock_adjustments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_items_ItemId",
                table: "supplier_items",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_items_SupplierId_ItemId",
                table: "supplier_items",
                columns: new[] { "SupplierId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_suppliers_Email",
                table: "suppliers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_RoleId",
                table: "user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_warehouses_Code",
                table: "warehouses",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "goods_receipt_lines");

            migrationBuilder.DropTable(
                name: "inventory_balances");

            migrationBuilder.DropTable(
                name: "inventory_transactions");

            migrationBuilder.DropTable(
                name: "stock_adjustments");

            migrationBuilder.DropTable(
                name: "supplier_items");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "goods_receipts");

            migrationBuilder.DropTable(
                name: "purchase_order_lines");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "purchase_orders");

            migrationBuilder.DropTable(
                name: "warehouses");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
