using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Api.Contracts.Request.Transaction;
using TransactionService.Api.Shared;
using TransactionService.Application.Transaction.Commands.CreateTransaction;
using TransactionService.Application.Transaction.Commands.DeleteTransaction;
using TransactionService.Application.Transaction.Commands.UpdateTransaction;
using TransactionService.Application.Transaction.Queries.GetAllTransactions;

namespace TransactionService.Api.Controllers;

/// <summary>
/// Controlador de transacciones de inventario
/// </summary>
/// <param name="mediator"></param>
[Route("api/[controller]")]
public class TransactionsController(ISender mediator) : ApiControllerBase
{
    /// <summary>
    /// Record a purchase or sale transaction
    /// </summary>

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateTransactionCommand(
            request.TransactionDate,
            request.Type,
            request.ProductId,
            request.Quantity,
            request.UnitPrice,
            request.Detail);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(id => Created($"/api/transactions/{id}", new { id }), HandleErrors);
    }

    /// <summary>
    /// Update a transaction
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTransaction(
        Guid id,
        [FromBody] UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTransactionCommand(
            id,
            request.TransactionDate,
            request.Quantity,
            request.UnitPrice,
            request.Detail);

        var result = await mediator.Send(command, cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(),HandleErrors);
    }

    /// <summary>
    /// Delete transaction and revert stock
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteTransaction(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTransactionCommand(id), cancellationToken);

        return result.Match<IActionResult>(_ => NoContent(),HandleErrors);
    }

    ///<summary>
    /// Get the transaction history using dynamic filters
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTransactions([FromQuery] GetAllTransactionsQuery query,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);

        return result.Match<IActionResult>(Ok,HandleErrors);
    }
}