using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionService.Application.Shared.Interfaces;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Repositories;
using TransactionService.Domain.Repositories.Shared;
using TransactionService.Domain.Transaction.ValueObjects;

namespace TransactionService.Application.Transaction.Commands.UpdateTransaction;

public class UpdateTransactionCommandHandler(ILogger<UpdateTransactionCommandHandler> logger,ITransactionRepository transactionRepository,IProductsApiClient productsApiClient,IUnitOfWork unitOfWork) : IRequestHandler<UpdateTransactionCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UpdateTransactionCommand request,CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(TransactionId.CreateUnique(request.Id), cancellationToken).ConfigureAwait(false);

        if (transaction is null)
        {
            logger.LogInformation("Transaction with id {TransactionId} not found", request.Id);
            return DomainErrors.Transaction.TransactionNotFound;
        }

        var oldDelta = transaction.StockDelta();

        var newDelta = transaction.Type == TransactionType.Purchase
            ? request.Quantity
            : -request.Quantity;

        var adjustment = newDelta - oldDelta;

        if (adjustment != 0)
        {
            var adjustResult = await productsApiClient.AdjustStockAsync(transaction.ProductId.Value, adjustment, cancellationToken).ConfigureAwait(false);

            if (adjustResult.IsError)
                return adjustResult.Errors;
        }

        try
        {
            transaction.Update(
                request.TransactionDate,
                request.Quantity,
                request.UnitPrice,
                request.Detail);

            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Transaction updated successfully with Id: {TransactionId}. Stock adjustment applied: {Adjustment}",request.Id, adjustment);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Error updating transaction {TransactionId}. Compensating stock adjustment of {Adjustment}",request.Id, adjustment);

            if (adjustment != 0)
            {
                var compensation = await productsApiClient.AdjustStockAsync(transaction.ProductId.Value, -adjustment, CancellationToken.None).ConfigureAwait(false);

                if (compensation.IsError)
                {
                    logger.LogCritical("COMPENSATION FAILED for product {ProductId}. Stock is inconsistent by {Adjustment} units. Manual intervention required.",transaction.ProductId.Value, adjustment);
                }
            }

            return DomainErrors.Transaction.TransactionUnexpectedError;
        }
    }
}