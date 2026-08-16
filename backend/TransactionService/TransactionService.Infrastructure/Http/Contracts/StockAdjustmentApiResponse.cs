namespace TransactionService.Infrastructure.Http.Contracts;

public record StockAdjustmentApiResponse(Guid ProductId, int NewStock);