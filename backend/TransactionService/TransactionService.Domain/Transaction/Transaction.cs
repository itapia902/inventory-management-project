using TransactionService.Domain.Enums;
using TransactionService.Domain.Primitives;
using TransactionService.Domain.Transaction.ValueObjects;

namespace TransactionService.Domain.Transaction;

public class Transaction : AggregateRoot<TransactionId>
{
    public DateTime TransactionDate { get; private set; }
    public TransactionType Type { get; private set; }
    public ProductId ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string? Detail { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedDateTime { get;  set; }
    public DateTime? UpdatedDateTime { get;  set; }

    private Transaction(TransactionId id, DateTime transactionDate, TransactionType type, ProductId productId, int quantity, decimal unitPrice,string? detail, bool isActive, DateTime createdDateTime) : base(id)
    {
        TransactionDate = transactionDate;
        Type = type;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
        Detail = detail;
        IsActive = isActive;
        CreatedDateTime = createdDateTime;
    }

    public static Transaction Create(DateTime transactionDate,TransactionType type, ProductId productId,int quantity,decimal unitPrice,string? detail)
        => new(TransactionId.CreateUnique(), transactionDate, type, productId, quantity, unitPrice, detail, true, DateTime.UtcNow);

    public void Update(DateTime transactionDate,int quantity, decimal unitPrice, string? detail)
    {
        TransactionDate = transactionDate;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = quantity * unitPrice;
        Detail = detail;
        UpdatedDateTime = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive) return;

        IsActive = false;
        UpdatedDateTime = DateTime.UtcNow;
    }
    public int StockDelta() => Type == TransactionType.Purchase ? Quantity : -Quantity;
}