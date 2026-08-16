using TransactionService.Domain.Primitives;

namespace TransactionService.Domain.Repositories.Shared;

public interface IBaseReadRepository<TEntity, in TId>
    where TEntity : Entity<TId>
{
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TId entityId, CancellationToken cancellationToken = default);
}