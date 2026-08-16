using ProductService.Domain.Primitives;

namespace ProductService.Domain.Product.ValueObjects;

public class ProductId : ValueObject
{
    public Guid Value { get; init; }

    private ProductId(Guid value)
    {
        Value = value;
    }    
    public static ProductId CreateUnique() => new(Guid.NewGuid());
    public static ProductId CreateUnique(Guid id) => new(id);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value.ToString();
}