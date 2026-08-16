using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Repositories;
using ProductService.Domain.Repositories.Shared;
using ProductService.Domain.Shared.Errors;
using ProductDomain = ProductService.Domain.Product.Product;

namespace ProductService.Application.Product.Commands.CreateProduct;

public class CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger,IUnitOfWork unitOfWork, IProductRepository productRepository) : IRequestHandler<CreateProductCommand, ErrorOr<Guid>>
{   
    public async Task<ErrorOr<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = ProductDomain.Create(
                request.Name,
                request.Description,
                request.Category,
                request.Price,
                request.ImageUrl,
                request.Stock);

            await productRepository.AddAsync(product, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("Product created successfully with Id: {ProductId}", product.Id.Value);

            return product.Id.Value;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}", nameof(CreateProductCommandHandler), request, ex.Message);
            return DomainErrors.Product.ProductUnexpectedError;
        }
    }
    
}
