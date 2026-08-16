using TransactionService.Domain.Primitives;
using TransactionService.Domain.Repositories.Shared;
using TransactionService.Domain.Shared;
using TransactionService.Domain.Transaction.ValueObjects;
using TransactionDomain = TransactionService.Domain.Transaction.Transaction;

namespace TransactionService.Domain.Repositories;

public interface ITransactionRepository : IBaseWriteRepository<TransactionDomain>
{
    Task<TransactionDomain?> GetByIdAsync(TransactionId id, CancellationToken cancellationToken = default);

    Task<ResultCriteria<TransactionDomain>> GetByParametersAsync(
        TransactionFilter filter,
        CancellationToken cancellationToken = default);
}