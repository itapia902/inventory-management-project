using ProductService.Domain.Product;
using ProductService.Domain.Product.ValueObjects;
using static ProductService.Domain.Product.Product;

namespace ProductService.Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductByParametersSpecification : Specification<Product, ProductId>
{
    public ProductByParametersSpecification(ProductFilter filter) : base(null)
    {
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            string searchTerm = filter.Name.Trim();
            AddCriteria(p => p.Name.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            string targetCategory = filter.Category.Trim();
            AddCriteria(p => p.Category == targetCategory);
        }

        if (filter.MinPrice.HasValue)
        {
            AddCriteria(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            AddCriteria(p => p.Price <= filter.MaxPrice.Value);
        }

        if (filter.MinStock.HasValue)
        {
            AddCriteria(p => p.Stock >= filter.MinStock.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy))
        {
            AddOrderByAsString($"{filter.SortBy} {filter.SortDirection}");
        }
        else
        {
            AddOrderByDescending(p => p.CreatedDateTime);
        }
    }
}