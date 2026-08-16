namespace ProductService.Api.Contracts.Request.Product;

public record UpdateProductRequest(
    string Name,
    string Description,
    string Category,
    decimal Price,
    string? ImageUrl);