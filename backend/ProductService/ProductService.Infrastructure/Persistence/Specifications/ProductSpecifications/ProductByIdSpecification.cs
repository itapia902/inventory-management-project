using ProductService.Domain.Product;
using ProductService.Domain.Product.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductByIdSpecification(ProductId productId) :
    Specification<Product, ProductId>(product => product.Id == productId);