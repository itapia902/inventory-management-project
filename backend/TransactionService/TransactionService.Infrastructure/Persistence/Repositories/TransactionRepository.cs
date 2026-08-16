using System.Linq.Dynamic.Core;
using TransactionService.Domain.Primitives;
using TransactionService.Domain.Repositories;
using TransactionService.Domain.Shared;
using TransactionService.Domain.Transaction;
using TransactionService.Domain.Transaction.ValueObjects;
using TransactionService.Infrastructure.Persistence.Repositories.Shared;
using TransactionService.Infrastructure.Persistence.Specifications.TransactionSpecifications;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.Infrastructure.Persistence.Repositories;

public class TransactionRepository(ApplicationDbContext dbContext)
    : BaseRepository<Transaction, TransactionId>(dbContext), ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(
        TransactionId id,
        CancellationToken cancellationToken = default) =>
        await ApplySpecification(new TransactionByIdSpecification(id))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

    public async Task<ResultCriteria<Transaction>> GetByParametersAsync(
        TransactionFilter filter,
        CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(new TransactionByParametersSpecification(filter));

        var totalItems = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new ResultCriteria<Transaction>(items, filter.Page, filter.PageSize, totalItems);
    }
}