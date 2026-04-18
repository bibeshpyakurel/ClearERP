using ClearErp.Domain.Common;

namespace ClearErp.Domain.Entities;

public sealed class Supplier : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    public ICollection<SupplierItem> SupplierItems { get; set; } = new List<SupplierItem>();

    public static Supplier Create(string name, string contactName, string email, string phone, string? notes = null)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 200);
        Guard.AgainstNullOrWhiteSpace(contactName, nameof(contactName), 120);
        Guard.AgainstInvalidEmail(email, nameof(email));
        Guard.AgainstNullOrWhiteSpace(phone, nameof(phone), 30);

        return new Supplier
        {
            Name = name.Trim(),
            ContactName = contactName.Trim(),
            Email = email.Trim(),
            Phone = phone.Trim(),
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim(),
            IsActive = true
        };
    }

    public void UpdateDetails(string name, string contactName, string email, string phone, string? notes = null)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), 200);
        Guard.AgainstNullOrWhiteSpace(contactName, nameof(contactName), 120);
        Guard.AgainstInvalidEmail(email, nameof(email));
        Guard.AgainstNullOrWhiteSpace(phone, nameof(phone), 30);

        Name = name.Trim();
        ContactName = contactName.Trim();
        Email = email.Trim();
        Phone = phone.Trim();
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        Touch();
    }

    public void Activate()
    {
        IsActive = true;
        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;
        Touch();
    }
}
