using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionService.Application.Shared.Interfaces;
using TransactionService.Application.Transaction.Commands.DeleteTransaction;
using TransactionService.Domain.Repositories;
using TransactionService.Domain.Repositories.Shared;
using TransactionService.Domain.Transaction.ValueObjects;

public class DeleteTransactionCommandHandler(ILogger<DeleteTransactionCommandHandler> logger,ITransactionRepository transactionRepository,IProductsApiClient productsApiClient,IUnitOfWork unitOfWork) : IRequestHandler<DeleteTransactionCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteTransactionCommand request,CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(TransactionId.CreateUnique(request.Id), cancellationToken).ConfigureAwait(false);

        if (transaction is null)
        {
            logger.LogInformation("Transaction with id {TransactionId} not found", request.Id);
            return DomainErrors.Transaction.TransactionNotFound;
        }

        var reversal = -transaction.StockDelta();
        var adjustResult = await productsApiClient.AdjustStockAsync(transaction.ProductId.Value, reversal, cancellationToken).ConfigureAwait(false);

        if (adjustResult.IsError)
            return adjustResult.Errors;

        try
        {
            transaction.Deactivate();
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Transaction deleted successfully with Id: {TransactionId}. Stock reversal applied: {Reversal}",request.Id, reversal);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Error deleting transaction {TransactionId}. Compensating stock reversal of {Reversal}",request.Id, reversal);
            var compensation = await productsApiClient.AdjustStockAsync(transaction.ProductId.Value, -reversal, CancellationToken.None).ConfigureAwait(false);

            if (compensation.IsError)
            {
                logger.LogCritical("COMPENSATION FAILED for product {ProductId}. Stock is inconsistent by {Reversal} units. Manual intervention required.",transaction.ProductId.Value, reversal);
            }
            return DomainErrors.Transaction.TransactionUnexpectedError;
        }
    }
}
