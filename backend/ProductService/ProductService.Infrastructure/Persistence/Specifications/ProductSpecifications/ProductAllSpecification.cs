using ProductService.Domain.Product;
using ProductService.Domain.Product.ValueObjects;

namespace ProductService.Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductAllSpecification() : Specification<Product, ProductId>(null);
