using ErrorOr;
using MediatR;

namespace ProductService.Application.Product.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string Category,
    decimal Price,
    string? ImageUrl) : IRequest<ErrorOr<Unit>>;