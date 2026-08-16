using MediatR;
using ErrorOr;

namespace ProductService.Application.Product.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    string Category,
    decimal Price,
    int Stock,
    string? ImageUrl) : IRequest<ErrorOr<Guid>>;


