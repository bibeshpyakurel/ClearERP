using FluentValidation;
using ClearErp.Api.Contracts.Inventory;

namespace ClearErp.Api.Validation.Inventory;

public sealed class IssueStockRequestValidator : AbstractValidator<IssueStockRequest>
{
    public IssueStockRequestValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty().WithMessage("Item is required.");
        RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("Warehouse is required.");
        RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location is required.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Issue quantity must be greater than 0.");
        RuleFor(x => x.ReferenceType)
            .NotEmpty().WithMessage("Reference type is required.")
            .MaximumLength(50).WithMessage("Reference type must be 50 characters or fewer.");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(500).WithMessage("Reason must be 500 characters or fewer.");
    }
}
