using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionService.Infrastructure.Http.Contracts;

public record ProductApiResponse(
    Guid Id,
    string Name,
    string Description,
    string Category,
    decimal Price,
    int Stock,
    string? ImageUrl);