namespace ProductService.Infrastructure.Persistence.Repositories.Shared;

public interface IBaseWriteRepository<in TEntity>
    where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity);
}