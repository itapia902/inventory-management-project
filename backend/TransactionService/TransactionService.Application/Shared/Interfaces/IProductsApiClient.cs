using ErrorOr;

namespace TransactionService.Application.Shared.Interfaces;

public interface IProductsApiClient
{
    Task<ErrorOr<ProductInfo>> GetProductAsync(
        Guid productId,
        CancellationToken cancellationToken = default);

    Task<ErrorOr<int>> AdjustStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);
}

