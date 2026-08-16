using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Product.Queries.QueryObjects;
using ProductService.Domain.Primitives;
using ProductService.Domain.Repositories;
using ProductService.Domain.Shared;
using ProductService.Domain.Shared.Errors;

namespace ProductService.Application.Product.Queries.GetProductByParameters;

public class GetProductByParametersQueryHandler(ILogger<GetProductByParametersQueryHandler> logger, IProductRepository productRepository)
    : IRequestHandler<GetProductByParametersQuery, ErrorOr<ResultCriteria<ProductResult>>>
{
    public async Task<ErrorOr<ResultCriteria<ProductResult>>> Handle(
        GetProductByParametersQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var filter = new ProductFilter(
                request.Name,
                request.Category,
                request.MinPrice,
                request.MaxPrice,
                request.MinStock,
                request.SortBy,
                request.SortDirection,
                request.Page,
                request.PageSize);

            var pagedProducts = await productRepository
                .GetByParametersAsync(filter, cancellationToken)
                .ConfigureAwait(false);

            var items = pagedProducts.items
                .Select(product => new ProductResult(
                    product.Id.Value,
                    product.Name,
                    product.Description,
                    product.Category,
                    product.Price,
                    product.Stock,
                    product.IsActive,
                    product.ImageUrl))
                .ToList();

            return new ResultCriteria<ProductResult>(
                items,
                pagedProducts.Page,
                pagedProducts.PageSize,
                pagedProducts.TotalItems);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on Handler: {HandlerName} with Request: {@HandlerRequest} and ExMessage: {ExMessage}",
                nameof(GetProductByParametersQueryHandler), request, ex.Message);

            return DomainErrors.Product.ProductUnexpectedError;
        }
    }
}