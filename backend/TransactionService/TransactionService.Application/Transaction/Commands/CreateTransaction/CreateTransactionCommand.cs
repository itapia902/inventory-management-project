using ErrorOr;
using MediatR;
using TransactionService.Domain.Enums;

namespace TransactionService.Application.Transaction.Commands.CreateTransaction;

public record CreateTransactionCommand(
    DateTime TransactionDate,
    TransactionType Type,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string? Detail) : IRequest<ErrorOr<Guid>>;
