namespace TransactionService.Api.Contracts.Request.Transaction;

public record UpdateTransactionRequest(
    DateTime TransactionDate,
    int Quantity,
    decimal UnitPrice,
    string? Detail);