using FluentValidation;
using ClearErp.Api.Contracts.PurchaseOrders;

namespace ClearErp.Api.Validation.PurchaseOrders;

public sealed class CreatePurchaseOrderRequestValidator : AbstractValidator<CreatePurchaseOrderRequest>
{
    public CreatePurchaseOrderRequestValidator()
    {
        RuleFor(x => x.PoNumber)
            .NotEmpty().WithMessage("PO number is required.")
            .MaximumLength(50).WithMessage("PO number must be 50 characters or fewer.");
        RuleFor(x => x.SupplierId).NotEmpty().WithMessage("Supplier is required.");
        RuleFor(x => x.OrderDate).NotEmpty().WithMessage("Order date is required.");
        RuleFor(x => x.Lines).NotEmpty().WithMessage("At least one purchase order line is required.");
        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(x => x.ItemId).NotEmpty().WithMessage("Item is required.");
            line.RuleFor(x => x.OrderedQuantity).GreaterThan(0).WithMessage("Ordered quantity must be greater than 0.");
            line.RuleFor(x => x.UnitCost).GreaterThanOrEqualTo(0).WithMessage("Unit cost cannot be negative.");
        });
    }
}
