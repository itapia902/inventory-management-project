using ErrorOr;
using MediatR;
using TransactionService.Application.Transaction.Queries.QueryObjects;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Primitives;

namespace TransactionService.Application.Transaction.Queries.GetAllTransactions;
public record GetAllTransactionsQuery(
    Guid? ProductId = null,
    TransactionType? Type = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    string? SortBy = null,
    string SortDirection = "desc",
    int Page = 1,
    int PageSize = 10) : IRequest<ErrorOr<ResultCriteria<TransactionResult>>>;