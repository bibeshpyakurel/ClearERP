using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();
    public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public static User Create(string email, string passwordHash, string fullName)
    {
        Guard.AgainstInvalidEmail(email, nameof(email));
        Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash), 500);
        Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName), 200);

        return new User
        {
            Email = email.Trim(),
            PasswordHash = passwordHash.Trim(),
            FullName = fullName.Trim(),
            IsActive = true
        };
    }

    public void UpdateProfile(string fullName, string email)
    {
        Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName), 200);
        Guard.AgainstInvalidEmail(email, nameof(email));

        FullName = fullName.Trim();
        Email = email.Trim();
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }
}
