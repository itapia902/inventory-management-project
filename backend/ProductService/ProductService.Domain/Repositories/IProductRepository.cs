using ProductService.Domain.Primitives;
using ProductService.Domain.Product.ValueObjects;
using ProductService.Domain.Repositories.Shared;
using ProductService.Domain.Shared;
using static ProductService.Domain.Product.Product;
using ProductDomain = ProductService.Domain.Product.Product;

namespace ProductService.Domain.Repositories;

public interface IProductRepository : IBaseReadRepository<ProductDomain, ProductId>,
    IBaseWriteRepository<ProductDomain>
{
    Task AddRangeAsync(IEnumerable<ProductDomain> entities, CancellationToken cancellationToken = default);
    Task<ResultCriteria<ProductDomain>> GetByParametersAsync(ProductFilter filter, CancellationToken cancellationToken = default);

}