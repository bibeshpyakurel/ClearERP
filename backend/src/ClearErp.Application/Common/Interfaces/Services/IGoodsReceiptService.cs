namespace ClearErp.Application.Common.Interfaces.Services;

public interface IGoodsReceiptService
{
    Task<GoodsReceiptDto> ReceiveAgainstPurchaseOrderAsync(
        Guid purchaseOrderId,
        string receiptNumber,
        Guid receivedByUserId,
        DateTime? receivedAtUtc,
        IReadOnlyCollection<PostGoodsReceiptLineRequest> lines,
        CancellationToken cancellationToken = default);
}
