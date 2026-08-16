using ErrorOr;
using MediatR;
using ProductService.Application.Product.Queries.QueryObjects;

namespace ProductService.Application.Product.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ErrorOr<ProductResult>>;
