namespace ProductService.Domain.Shared;

public record ProductFilter(
    string? Name = null, 
    string? Category = null, 
    decimal? MinPrice = null, 
    decimal? MaxPrice = null, 
    int? MinStock = null, 
    string? SortBy = null, 
    string SortDirection = "asc", 
    int Page = 1, 
    int PageSize = 10);