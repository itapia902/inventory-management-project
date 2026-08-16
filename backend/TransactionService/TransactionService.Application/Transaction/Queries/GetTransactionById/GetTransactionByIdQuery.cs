using ErrorOr;
using MediatR;
using TransactionService.Application.Transaction.Queries.QueryObjects;

namespace TransactionService.Application.Transaction.Queries.GetTransactionById;

public record GetTransactionByIdQuery(Guid Id) : IRequest<ErrorOr<TransactionResult>>;