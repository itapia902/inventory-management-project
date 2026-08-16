using ErrorOr;
using MediatR;

namespace TransactionService.Application.Transaction.Commands.DeleteTransaction;

public record DeleteTransactionCommand(Guid Id) : IRequest<ErrorOr<Unit>>;