using ErrorOr;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using TransactionService.Application.Shared;
using TransactionService.Application.Shared.Interfaces;
using TransactionService.Infrastructure.Http.Contracts;

namespace TransactionService.Infrastructure.Http;

public class ProductsApiClient(
    HttpClient httpClient,
    ILogger<ProductsApiClient> logger) : IProductsApiClient
{
    public async Task<ErrorOr<ProductInfo>> GetProductAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient
            .GetAsync($"/api/products/{productId}", cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return DomainErrors.Transaction.ProductNotFound;

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ProductService returned {StatusCode} for GET product {ProductId}",
                response.StatusCode, productId);

            return DomainErrors.Transaction.ProductServiceUnavailable;
        }

        var product = await response.Content
            .ReadFromJsonAsync<ProductApiResponse>(cancellationToken)
            .ConfigureAwait(false);

        if (product is null)
            return DomainErrors.Transaction.ProductServiceUnavailable;

        return new ProductInfo(product.Id, product.Name, product.Price, product.Stock);
    }

    public async Task<ErrorOr<int>> AdjustStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient
            .PostAsJsonAsync($"/api/products/{productId}/stock-adjustments",
                new { quantity }, cancellationToken)
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return DomainErrors.Transaction.ProductNotFound;

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            var problem = await response.Content
                .ReadFromJsonAsync<ProblemDetailsResponse>(cancellationToken)
                .ConfigureAwait(false);

            return Error.Conflict(
                "Transaction.InsufficientStock",
                problem?.Title ?? "Stock insuficiente para realizar la venta");
        }

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ProductService returned {StatusCode} adjusting stock for {ProductId}",
                response.StatusCode, productId);

            return DomainErrors.Transaction.ProductServiceUnavailable;
        }

        var result = await response.Content
            .ReadFromJsonAsync<StockAdjustmentApiResponse>(cancellationToken)
            .ConfigureAwait(false);

        return result?.NewStock ?? 0;
    }
}