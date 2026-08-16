using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionService.Application.Shared.Interfaces;
using TransactionService.Application.Transaction.Queries.QueryObjects;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Primitives;
using TransactionService.Domain.Repositories;
using TransactionService.Domain.Shared;

namespace TransactionService.Application.Transaction.Queries.GetAllTransactions;

public class GetTransactionsQueryHandler(ILogger<GetTransactionsQueryHandler> logger,ITransactionRepository transactionRepository,IProductsApiClient productsApiClient)
    : IRequestHandler<GetAllTransactionsQuery, ErrorOr<ResultCriteria<TransactionResult>>>
{
    public async Task<ErrorOr<ResultCriteria<TransactionResult>>> Handle(GetAllTransactionsQuery request,CancellationToken cancellationToken)
    {
        try
        {
            var filter = new TransactionFilter(
                request.ProductId,
                request.Type,
                request.DateFrom,
                request.DateTo,
                request.SortBy,
                request.SortDirection,
                request.Page,
                request.PageSize);

            var paged = await transactionRepository.GetByParametersAsync(filter, cancellationToken).ConfigureAwait(false);
            
            var productIds = paged.Items
                .Select(t => t.ProductId.Value)
                .Distinct()
                .ToList();

            var productResults = await Task.WhenAll(productIds.Select(id => productsApiClient.GetProductAsync(id, cancellationToken))).ConfigureAwait(false);

            var products = productResults
                .Where(result => !result.IsError)
                .Select(result => result.Value)
                .ToDictionary(product => product.Id);

            var items = paged.Items.Select(transaction =>
            {
                products.TryGetValue(transaction.ProductId.Value, out var product);

                return new TransactionResult(
                    transaction.Id.Value,
                    transaction.TransactionDate,
                    transaction.Type,
                    transaction.Type == TransactionType.Purchase ? "Compra" : "Venta",
                    transaction.ProductId.Value,
                    product?.Name ?? "(producto no disponible)",
                    product?.Stock ?? 0,
                    transaction.Quantity,
                    transaction.UnitPrice,
                    transaction.TotalPrice,
                    transaction.Detail);
            }).ToList();

            return new ResultCriteria<TransactionResult>(items, paged.Page, paged.PageSize, paged.TotalItems);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}",nameof(GetTransactionsQueryHandler), request, ex.Message);

            return DomainErrors.Transaction.TransactionUnexpectedError;
        }
    }
}