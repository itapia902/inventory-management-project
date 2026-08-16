using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Primitives;
using ProductService.Infrastructure.Persistence.Specifications;
namespace ProductService.Infrastructure.Persistence.Repositories.Shared;

public abstract class BaseRepository<TEntity, TId>(ApplicationDbContext dbContext) : BaseWriteRepository<TEntity>(dbContext)
    where TEntity : Entity<TId>
{
    protected IQueryable<TEntity> ApplySpecification(Specification<TEntity, TId> specification) =>
        SpecificationEvaluator.GetQuery(DbContext.Set<TEntity>(), specification);

}