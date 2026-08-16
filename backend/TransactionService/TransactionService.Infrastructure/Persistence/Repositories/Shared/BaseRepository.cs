using TransactionService.Domain.Primitives;
using TransactionService.Infrastructure.Persistence.Specifications;
namespace TransactionService.Infrastructure.Persistence.Repositories.Shared;

public abstract class BaseRepository<TEntity, TId>(ApplicationDbContext dbContext) : BaseWriteRepository<TEntity>(dbContext)
    where TEntity : Entity<TId>
{
    protected IQueryable<TEntity> ApplySpecification(Specification<TEntity, TId> specification) =>
        SpecificationEvaluator.GetQuery(DbContext.Set<TEntity>(), specification);

}