using ErrorOr;
using MediatR;

namespace ProductService.Application.Product.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<ErrorOr<Unit>>;