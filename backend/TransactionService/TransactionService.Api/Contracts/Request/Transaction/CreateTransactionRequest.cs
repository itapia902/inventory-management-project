using TransactionService.Domain.Enums;

namespace TransactionService.Api.Contracts.Request.Transaction;

public record CreateTransactionRequest(
    DateTime TransactionDate,
    TransactionType Type,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string? Detail);