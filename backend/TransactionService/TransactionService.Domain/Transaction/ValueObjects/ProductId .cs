using TransactionService.Domain.Primitives;

namespace TransactionService.Domain.Transaction.ValueObjects;

public class ProductId : ValueObject
{
    public Guid Value { get; }

    private ProductId(Guid value) => Value = value;

    public static ProductId CreateUnique(Guid id) => new(id);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}