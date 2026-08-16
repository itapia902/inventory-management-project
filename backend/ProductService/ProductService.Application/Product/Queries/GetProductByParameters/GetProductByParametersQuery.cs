using ErrorOr;
using MediatR;
using ProductService.Application.Product.Queries.QueryObjects;
using ProductService.Domain.Primitives;

namespace ProductService.Application.Product.Queries.GetProductByParameters;

public record GetProductByParametersQuery(
    string? Name = null,
    string? Category = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    int? MinStock = null,
    string? SortBy = null,
    string SortDirection = "asc",
    int Page = 1,
    int PageSize = 10) : IRequest<ErrorOr<ResultCriteria<ProductResult>>>;