using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionService.Application.Shared.Interfaces;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Repositories;
using TransactionService.Domain.Repositories.Shared;
using TransactionService.Domain.Transaction.ValueObjects;
using TransactionDomain = TransactionService.Domain.Transaction.Transaction;

namespace TransactionService.Application.Transaction.Commands.CreateTransaction;

public class CreateTransactionCommandHandler(ILogger<CreateTransactionCommandHandler> logger,ITransactionRepository transactionRepository,IProductsApiClient productsApiClient, IUnitOfWork unitOfWork) : IRequestHandler<CreateTransactionCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var stockDelta = request.Type == TransactionType.Purchase
            ? request.Quantity
            : -request.Quantity;

        // 1. Ajustar el stock en ProductService. Aquí se valida si alcanza.
        var adjustResult = await productsApiClient
            .AdjustStockAsync(request.ProductId, stockDelta, cancellationToken)
            .ConfigureAwait(false);

        if (adjustResult.IsError)
            return adjustResult.Errors;

        // 2. Persistir la transacción
        try
        {
            var transaction = TransactionDomain.Create(
                request.TransactionDate,
                request.Type,
                ProductId.CreateUnique(request.ProductId),
                request.Quantity,
                request.UnitPrice,
                request.Detail);

            await transactionRepository.AddAsync(transaction, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation(
                "Transaction created successfully with Id: {TransactionId} for product {ProductId}",
                transaction.Id.Value, request.ProductId);

            return transaction.Id.Value;
        }
        catch (Exception ex)
        {
            // 3. Compensación: revertir el ajuste de stock
            logger.LogError(ex,
                "Error saving transaction for product {ProductId}. Compensating stock adjustment of {StockDelta}",
                request.ProductId, stockDelta);

            var compensation = await productsApiClient
                .AdjustStockAsync(request.ProductId, -stockDelta, CancellationToken.None)
                .ConfigureAwait(false);

            if (compensation.IsError)
            {
                logger.LogCritical(
                    "COMPENSATION FAILED for product {ProductId}. Stock is inconsistent by {StockDelta} units. Manual intervention required.",
                    request.ProductId, stockDelta);
            }

            return DomainErrors.Transaction.TransactionUnexpectedError;
        }
    }
}