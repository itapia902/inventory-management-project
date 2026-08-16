using TransactionService.Domain.Enums;

namespace TransactionService.Application.Transaction.Queries.QueryObjects;

public record TransactionResult(
    Guid Id,
    DateTime TransactionDate,
    TransactionType Type,
    string TypeName,
    Guid ProductId,
    string ProductName,
    int ProductStock,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    string? Detail);