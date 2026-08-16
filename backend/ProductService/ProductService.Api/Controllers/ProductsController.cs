using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Contracts.Request.Product;
using ProductService.Api.Shared;
using ProductService.Application.Product.Commands.CreateProduct;
using ProductService.Application.Product.Commands.DeleteProduct;
using ProductService.Application.Product.Commands.UpdateProduct;
using ProductService.Application.Product.Commands.UpdateProductStock;
using ProductService.Application.Product.Queries.GetProductById;
using ProductService.Application.Product.Queries.GetProductByParameters;

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
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateProduct( [FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            request.Name,
            request.Description,
            request.Category,
            request.Price,
            request.Stock,
            request.ImageUrl);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(id => CreatedAtAction(nameof(GetProductById), new { id }, new { id }),HandleErrors);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct( Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(
            id,
            request.Name,
            request.Description,
            request.Category,
            request.Price,
            request.ImageUrl);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(),HandleErrors);
    }

    /// <summary>
    /// Update the stock of a product
    /// </summary>    
    [HttpPost("{id:guid}/stock-adjustments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateProductStock(Guid id, [FromBody] UpdateProductStockRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductStockCommand(id, request.Quantity);
        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(newStock => Ok(new { productId = id, newStock }),HandleErrors);
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return result.Match<IActionResult>(Ok,HandleErrors);
    }

    /// <summary>
    /// Get a list of products with dynamic filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductByParametersQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(Ok,HandleErrors);
    }

    /// <summary>
    /// Delete a product (logical deletion)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductCommand(id), cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(),HandleErrors);
    }
}