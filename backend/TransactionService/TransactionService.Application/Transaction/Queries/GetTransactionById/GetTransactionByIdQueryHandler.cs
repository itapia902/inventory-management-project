using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionService.Application.Shared.Interfaces;
using TransactionService.Application.Transaction.Queries.QueryObjects;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Repositories;
using TransactionService.Domain.Transaction.ValueObjects;

namespace TransactionService.Application.Transaction.Queries.GetTransactionById;

public class GetTransactionByIdQueryHandler(ILogger<GetTransactionByIdQueryHandler> logger,ITransactionRepository transactionRepository,IProductsApiClient productsApiClient)
    : IRequestHandler<GetTransactionByIdQuery, ErrorOr<TransactionResult>>
{
    public async Task<ErrorOr<TransactionResult>> Handle(GetTransactionByIdQuery request,CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await transactionRepository.GetByIdAsync(TransactionId.CreateUnique(request.Id), cancellationToken).ConfigureAwait(false);

            if (transaction is null)
            {
                logger.LogInformation("Transaction with id {TransactionId} not found", request.Id);
                return DomainErrors.Transaction.TransactionNotFound;
            }

            var productResult = await productsApiClient.GetProductAsync(transaction.ProductId.Value, cancellationToken).ConfigureAwait(false);
            var productName = productResult.IsError ? "(producto no disponible)" : productResult.Value.Name;
            var productStock = productResult.IsError ? 0 : productResult.Value.Stock;

            return new TransactionResult(
                transaction.Id.Value,
                transaction.TransactionDate,
                transaction.Type,
                transaction.Type == TransactionType.Purchase ? "Compra" : "Venta",
                transaction.ProductId.Value,
                productName,
                productStock,
                transaction.Quantity,
                transaction.UnitPrice,
                transaction.TotalPrice,
                transaction.Detail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}",nameof(GetTransactionByIdQueryHandler), request, ex.Message);
            return DomainErrors.Transaction.TransactionUnexpectedError;
        }
    }
}