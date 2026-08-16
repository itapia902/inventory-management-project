using TransactionService.Domain.Shared;
using TransactionService.Domain.Transaction.ValueObjects;
using TransactionService.Domain.Transaction;

namespace TransactionService.Infrastructure.Persistence.Specifications.TransactionSpecifications;

public class TransactionByParametersSpecification : Specification<Transaction, TransactionId>
{
    public TransactionByParametersSpecification(TransactionFilter filter) : base(null)
    {
        if (filter.ProductId is not null)
        {
            var productId = ProductId.CreateUnique(filter.ProductId.Value);
            AddCriteria(t => t.ProductId == productId);
        }

        if (filter.Type is not null)
        {
            var type = filter.Type.Value;
            AddCriteria(t => t.Type == type);
        }

        if (filter.DateFrom is not null)
        {
            var dateFrom = filter.DateFrom.Value.Date;
            AddCriteria(t => t.TransactionDate >= dateFrom);
        }

        if (filter.DateTo is not null)
        {
            var dateTo = filter.DateTo.Value.Date.AddDays(1).AddTicks(-1);
            AddCriteria(t => t.TransactionDate <= dateTo);
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy))
            AddOrderByAsString($"{filter.SortBy} {filter.SortDirection}");
        else
            AddOrderByDescending(t => t.TransactionDate);
    }
}