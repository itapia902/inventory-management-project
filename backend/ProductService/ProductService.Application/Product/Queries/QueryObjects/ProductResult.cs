namespace ProductService.Application.Product.Queries.QueryObjects;

public record ProductResult(
    Guid Id,
    string Name,
    string Description,
    string Category,
    decimal Price,
    int Stock,
    bool IsActive,
    string? ImageUrl);
