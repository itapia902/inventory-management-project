using TransactionService.Domain.Enums;

namespace TransactionService.Domain.Shared;

public record TransactionFilter(
    Guid? ProductId = null,
    TransactionType? Type = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    string? SortBy = null,
    string SortDirection = "desc",
    int Page = 1,
    int PageSize = 10);