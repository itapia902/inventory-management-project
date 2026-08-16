namespace TransactionService.Infrastructure.Persistence.Repositories.Shared;

public abstract class BaseWriteRepository<TEntity>(ApplicationDbContext dbContext) : IBaseWriteRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);

    public Task UpdateAsync(TEntity entity) =>
        Task.FromResult(DbContext.Set<TEntity>().Update(entity));
}