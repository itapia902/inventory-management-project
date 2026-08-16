using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Product.Queries.QueryObjects;
using ProductService.Domain.Product.ValueObjects;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared.Errors;

namespace ProductService.Application.Product.Queries.GetProductById;

public class GetProductByIdQueryHandler(
    ILogger<GetProductByIdQueryHandler> logger,
    IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductResult>>
{
    public async Task<ErrorOr<ProductResult>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.GetByIdAsync(ProductId.CreateUnique(request.Id), cancellationToken).ConfigureAwait(false);

            if (product is null)
            {
                logger.LogInformation("Product with id {ProductId} not found", request.Id);
                return DomainErrors.Product.ProductNotFound;
            }

            return new ProductResult(
                product.Id.Value,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                product.Stock,
                product.IsActive,
                product.ImageUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}",
                nameof(GetProductByIdQueryHandler), request, ex.Message);

            return DomainErrors.Product.ProductUnexpectedError;
        }
    }
}