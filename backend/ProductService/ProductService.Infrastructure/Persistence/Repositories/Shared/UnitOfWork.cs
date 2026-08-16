using ProductService.Domain.Repositories.Shared;

namespace ProductService.Infrastructure.Persistence.Repositories.Shared;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {        
        return await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
    
}