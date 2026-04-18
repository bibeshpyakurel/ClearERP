using ClearErp.Domain.Common;
using ClearErp.Domain.Entities;

namespace ClearErp.UnitTests.Domain;

public sealed class SupplierTests
{
    [Fact]
    public void Create_Should_SetNotesAndActiveStatus()
    {
        var supplier = Supplier.Create("Northwind", "Taylor", "taylor@example.com", "555-1234", "Preferred vendor");

        Assert.Equal("Northwind", supplier.Name);
        Assert.Equal("Preferred vendor", supplier.Notes);
        Assert.True(supplier.IsActive);
    }

    [Fact]
    public void Create_Should_Throw_When_EmailIsInvalid()
    {
        var action = () => Supplier.Create("Northwind", "Taylor", "bad-email", "555-1234");

        var exception = Assert.Throws<DomainException>(action);
        Assert.Equal("email must be a valid email address.", exception.Message);
    }

    [Fact]
    public void UpdateDetails_Should_UpdateAllEditableFields()
    {
        var supplier = Supplier.Create("Northwind", "Taylor", "taylor@example.com", "555-1234");

        supplier.UpdateDetails("Contoso", "Jordan", "jordan@example.com", "555-9999", "Updated");

        Assert.Equal("Contoso", supplier.Name);
        Assert.Equal("Jordan", supplier.ContactName);
        Assert.Equal("jordan@example.com", supplier.Email);
        Assert.Equal("555-9999", supplier.Phone);
        Assert.Equal("Updated", supplier.Notes);
        Assert.NotNull(supplier.UpdatedAt);
    }

    [Fact]
    public void ActivateDeactivate_Should_ToggleStatus()
    {
        var supplier = Supplier.Create("Northwind", "Taylor", "taylor@example.com", "555-1234");

        supplier.Deactivate();
        Assert.False(supplier.IsActive);

        supplier.Activate();
        Assert.True(supplier.IsActive);
    }
}
