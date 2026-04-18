using FluentValidation;
using ClearErp.Api.Contracts.Suppliers;

namespace ClearErp.Api.Validation.Suppliers;

public sealed class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Supplier name is required.")
            .MaximumLength(200).WithMessage("Supplier name must be 200 characters or fewer.");
        RuleFor(x => x.ContactName)
            .NotEmpty().WithMessage("Primary contact is required.")
            .MaximumLength(120).WithMessage("Primary contact must be 120 characters or fewer.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Supplier email is required.")
            .EmailAddress().WithMessage("Enter a valid supplier email address.")
            .MaximumLength(200).WithMessage("Supplier email must be 200 characters or fewer.");
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(30).WithMessage("Phone number must be 30 characters or fewer.");
        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes must be 1000 characters or fewer.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ItemId).NotEmpty().WithMessage("Mapped item is required.");
            item.RuleFor(i => i.SupplierSku)
                .NotEmpty().WithMessage("Supplier SKU is required for mapped items.")
                .MaximumLength(64).WithMessage("Supplier SKU must be 64 characters or fewer.");
        });
    }
}
