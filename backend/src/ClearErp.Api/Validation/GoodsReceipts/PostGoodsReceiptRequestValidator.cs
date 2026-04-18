using FluentValidation;
using ClearErp.Api.Contracts.GoodsReceipts;

namespace ClearErp.Api.Validation.GoodsReceipts;

public sealed class PostGoodsReceiptRequestValidator : AbstractValidator<PostGoodsReceiptRequest>
{
    public PostGoodsReceiptRequestValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty().WithMessage("Purchase order is required.");
        RuleFor(x => x.ReceiptNumber)
            .NotEmpty().WithMessage("Receipt number is required.")
            .MaximumLength(50).WithMessage("Receipt number must be 50 characters or fewer.");
        RuleFor(x => x.Lines).NotEmpty().WithMessage("At least one receipt line is required.");
        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(x => x.PurchaseOrderLineId).NotEmpty().WithMessage("Purchase order line is required.");
            line.RuleFor(x => x.ItemId).NotEmpty().WithMessage("Item is required.");
            line.RuleFor(x => x.ReceivedQuantity).GreaterThan(0).WithMessage("Received quantity must be greater than 0.");
            line.RuleFor(x => x.WarehouseId).NotEmpty().WithMessage("Warehouse is required.");
            line.RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location is required.");
        });
    }
}
