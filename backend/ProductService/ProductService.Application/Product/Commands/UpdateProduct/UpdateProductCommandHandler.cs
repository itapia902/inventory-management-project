using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Product.ValueObjects;
using ProductService.Domain.Repositories;
using ProductService.Domain.Repositories.Shared;
using ProductService.Domain.Shared.Errors;

namespace ProductService.Application.Product.Commands.UpdateProduct;

public class UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger, IUnitOfWork unitOfWork,
    IProductRepository productRepository) : IRequestHandler<UpdateProductCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {            
            var product = await productRepository.GetByIdAsync(ProductId.CreateUnique(request.Id), cancellationToken).ConfigureAwait(false);

            if (product is null)
            {
                logger.LogInformation("Product with id {@ProductId} not found", request.Id);
                return DomainErrors.Product.ProductNotFound;
            }

            product.Update(
                request.Name,
                request.Description,
                request.Category,
                request.Price,
                request.ImageUrl);

            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Product updated successfully with Id: {ProductId}", product.Id.Value);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}", nameof(UpdateProductCommandHandler), request, ex.Message);
            return DomainErrors.Product.ProductUnexpectedError;
        }
    }
}
