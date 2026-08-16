using ErrorOr;
using MediatR;

namespace ProductService.Application.Product.Commands.UpdateProductStock;

public record UpdateProductStockCommand(
    Guid ProductId,
    int Quantity) : IRequest<ErrorOr<int>>;
