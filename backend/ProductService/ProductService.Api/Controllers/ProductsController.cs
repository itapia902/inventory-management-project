using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Contracts.Request.Product;
using ProductService.Api.Shared;
using ProductService.Application.Product.Commands.CreateProduct;

namespace ProductService.Api.Controllers;

/// <summary>
/// Controlador de productos
/// </summary>
/// <param name="mediator"></param>
[Route("api/[controller]")]
public class ProductsController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// Create new product
    /// </summary>
    /// 
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            request.Category,
            request.Price,
            request.Stock,
            request.ImageUrl);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(
            id => Created($"/api/products/{id}", new { id }),
            HandleErrors);
    }
}