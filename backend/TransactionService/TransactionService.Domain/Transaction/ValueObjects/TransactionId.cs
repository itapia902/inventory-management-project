using System;
using System.Collections.Generic;
using System.Text;
using TransactionService.Domain.Primitives;

namespace TransactionService.Domain.Transaction.ValueObjects;

public class TransactionId : ValueObject
{
    public Guid Value { get; init; }

    private TransactionId(Guid value)
    {
        Value = value;
    }
    public static TransactionId CreateUnique() => new(Guid.NewGuid());
    public static TransactionId CreateUnique(Guid id) => new(id);
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public override string ToString() => Value.ToString();
}