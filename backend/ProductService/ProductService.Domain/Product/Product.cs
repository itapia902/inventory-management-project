using ProductService.Domain.Primitives;
using ProductService.Domain.Product.ValueObjects;

namespace ProductService.Domain.Product;

public class Product : AggregateRoot<ProductId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }  
    public string? ImageUrl { get; private set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? UpdatedDateTime { get; set; }

    private Product (ProductId id, string name, string description, string category, decimal price, string? imageUrl,int stock, bool isActive) :base(id)
    {
        Name = name;
        Description = description;
        Category = category;
        Price = price;
        Stock = stock;
        ImageUrl = imageUrl;
        IsActive = isActive;
    }
    public static Product Create(string name, string description, string category, decimal price, string? imageUrl, int stock) 
        =>new (ProductId.CreateUnique(),  name,  description,  category,  price,  imageUrl,  stock, true);

    public record ProductFilter(string? Name = null, string? Category = null, decimal? MinPrice = null, decimal? MaxPrice = null, int? MinStock = null, string? SortBy = null, string SortDirection = "asc", int Page = 1, int PageSize = 10);
}
