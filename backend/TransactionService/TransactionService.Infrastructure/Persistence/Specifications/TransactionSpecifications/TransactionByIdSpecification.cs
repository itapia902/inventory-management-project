using TransactionService.Domain.Transaction;
using TransactionService.Domain.Transaction.ValueObjects;

namespace TransactionService.Infrastructure.Persistence.Specifications.TransactionSpecifications;

public class TransactionByIdSpecification(TransactionId transactionId) :
    Specification<Transaction, TransactionId>(transaction => transaction.Id == transactionId);