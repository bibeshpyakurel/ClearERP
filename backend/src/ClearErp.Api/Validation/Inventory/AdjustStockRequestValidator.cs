using FluentValidation;
using ClearErp.Api.Contracts.Inventory;

namespace ClearErp.Api.Validation.Inventory;

public sealed class AdjustStockRequestValidator : AbstractValidator<AdjustStockRequest>
{
    public AdjustStockRequestValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty().WithMessage("Item is required.");
        RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("Warehouse is required.");
        RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location is required.");
        RuleFor(x => x.QuantityDelta).NotEqual(0).WithMessage("Adjustment quantity cannot be zero.");
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(500).WithMessage("Reason must be 500 characters or fewer.");
    }
}
