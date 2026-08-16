using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Product.ValueObjects;
using ProductService.Domain.Repositories;
using ProductService.Domain.Repositories.Shared;
using ProductService.Domain.Shared.Errors;

namespace ProductService.Application.Product.Commands.DeleteProduct;

public class DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger, IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<DeleteProductCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle( DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.GetByIdAsync(ProductId.CreateUnique(request.Id), cancellationToken).ConfigureAwait(false);

            if (product is null)
            {
                logger.LogInformation("Product with id {ProductId} not found", request.Id);
                return DomainErrors.Product.ProductNotFound;
            }

            product.Deactivate();

            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Product deactivated successfully with Id: {ProductId}", request.Id);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}",
                nameof(DeleteProductCommandHandler), request, ex.Message);

            return DomainErrors.Product.ProductUnexpectedError;


        }
    }
}
