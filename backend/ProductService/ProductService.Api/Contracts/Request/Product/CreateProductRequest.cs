namespace ProductService.Api.Contracts.Request.Product;

public record CreateProductRequest(
    string Name,
    string Description,
    string Category,
    decimal Price,
    int Stock,
    string? ImageUrl);