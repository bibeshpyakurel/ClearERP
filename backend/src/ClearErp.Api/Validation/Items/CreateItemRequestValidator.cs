using FluentValidation;
using ClearErp.Api.Contracts.Items;

namespace ClearErp.Api.Validation.Items;

public sealed class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(64).WithMessage("SKU must be 64 characters or fewer.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Item name is required.")
            .MaximumLength(200).WithMessage("Item name must be 200 characters or fewer.");
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be 1000 characters or fewer.");
        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unit is required.")
            .MaximumLength(32).WithMessage("Unit must be 32 characters or fewer.");
        RuleFor(x => x.ReorderLevel)
            .GreaterThanOrEqualTo(0).WithMessage("Reorder level cannot be negative.");
        RuleFor(x => x.StandardCost)
            .GreaterThanOrEqualTo(0).WithMessage("Standard cost cannot be negative.");
    }
}
