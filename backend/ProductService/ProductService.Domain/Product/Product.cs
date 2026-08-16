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
    public DateTime CreatedDateTime { get; private set; }
    public DateTime? UpdatedDateTime { get; private set; }

    private Product (ProductId id, string name, string description, string category, decimal price, string? imageUrl,int stock, bool isActive, DateTime createdDateTime) :base(id)
    {
        Name = name;
        Description = description;
        Category = category;
        Price = price;
        Stock = stock;
        ImageUrl = imageUrl;
        IsActive = isActive;
        CreatedDateTime = createdDateTime;
    }
    public static Product Create(string name, string description, string category, decimal price, string? imageUrl, int stock) 
        =>new (ProductId.CreateUnique(),  name,  description,  category,  price,  imageUrl,  stock, true,DateTime.UtcNow);

    public void Update(string name, string description, string category, decimal price, string? imageUrl)
    {
        if (Name == name && Description == description && Category == category && Price == price && ImageUrl == imageUrl)
            return;

        Name = name;
        Description = description;
        Category = category;
        Price = price;
        ImageUrl = imageUrl;
        UpdatedDateTime = DateTime.UtcNow;
    }
    public void UpdateStock(int stock)
    {
        Stock = stock;
    }
    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        UpdatedDateTime = DateTime.UtcNow;
    }
}
