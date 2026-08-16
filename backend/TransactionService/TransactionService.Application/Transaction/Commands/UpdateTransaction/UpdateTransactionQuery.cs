using ErrorOr;
using MediatR;

namespace TransactionService.Application.Transaction.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
    Guid Id,
    DateTime TransactionDate,
    int Quantity,
    decimal UnitPrice,
    string? Detail) : IRequest<ErrorOr<Unit>>;