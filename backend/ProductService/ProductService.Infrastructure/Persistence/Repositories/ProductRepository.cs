using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Primitives;
using ProductService.Domain.Product;
using ProductService.Domain.Product.ValueObjects;
using ProductService.Domain.Repositories;
using ProductService.Infrastructure.Persistence.Repositories.Shared;
using ProductService.Infrastructure.Persistence.Specifications.ProductSpecifications;
using System.Linq.Dynamic.Core;
using static ProductService.Domain.Product.Product;

namespace ProductService.Infrastructure.Persistence.Repositories;

public class ProductRepository(ApplicationDbContext dbContext)
: BaseRepository<Product, ProductId>(dbContext), IProductRepository
{

    public async Task AddRangeAsync(IEnumerable<Product> entities, CancellationToken cancellationToken = default) =>
        await DbContext.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await ApplySpecification(new ProductAllSpecification()).ToListAsync(cancellationToken).ConfigureAwait(false);
    
    public async Task<Product?> GetByIdAsync(ProductId entityId, CancellationToken cancellationToken = default) =>
        await ApplySpecification(new ProductByIdSpecification(entityId)).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

    public async Task<ResultCriteria<Product>> GetByParametersAsync(
     ProductFilter filter,
     CancellationToken cancellationToken = default)
    {
        var specificationQuery = ApplySpecification(new ProductByParametersSpecification(filter));

        int totalRecords = await specificationQuery.CountAsync(cancellationToken);

        var pagedData = await specificationQuery
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new ResultCriteria<Product>(pagedData, filter.Page, filter.PageSize, totalRecords);
    }

}