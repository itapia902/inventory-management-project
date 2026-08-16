using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Product.ValueObjects;
using ProductService.Domain.Repositories;
using ProductService.Domain.Repositories.Shared;
using ProductService.Domain.Shared.Errors;

namespace ProductService.Application.Product.Commands.UpdateProductStock;

public class UpdateProductStockCommandHandler( ILogger<UpdateProductStockCommandHandler> logger, IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<UpdateProductStockCommand, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(
        UpdateProductStockCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository
                .GetByIdAsync(ProductId.CreateUnique(request.ProductId), cancellationToken)
                .ConfigureAwait(false);

            if (product is null)
            {
                logger.LogInformation("Product with id {ProductId} not found", request.ProductId);
                return DomainErrors.Product.ProductNotFound;
            }

            var newStock = product.Stock + request.Quantity;

            if (newStock < 0)
            {
                logger.LogInformation("Insufficient stock for product {ProductId}. Available: {Available}, requested: {Requested}",
                    request.ProductId, product.Stock, Math.Abs(request.Quantity));

                return DomainErrors.Product.InsufficientStock(product.Stock, Math.Abs(request.Quantity));
            }

            product.UpdateStock(newStock);

            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Stock adjusted for product {ProductId}. Quantity: {Quantity}, new stock: {Stock}",
                request.ProductId, request.Quantity, product.Stock);

            return product.Stock;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}",
                nameof(UpdateProductStockCommandHandler), request, ex.Message);

            return DomainErrors.Product.ProductUnexpectedError;
        }
    }
}