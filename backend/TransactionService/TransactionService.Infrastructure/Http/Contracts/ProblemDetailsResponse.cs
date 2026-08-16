namespace TransactionService.Infrastructure.Http.Contracts;

public record ProblemDetailsResponse(string? Title, int? Status);