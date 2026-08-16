using Microsoft.EntityFrameworkCore;
using TransactionDomain = TransactionService.Domain.Transaction.Transaction;

namespace TransactionService.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TransactionDomain> Transactions => Set<TransactionDomain>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("transactions");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<TransactionDomain>().HasQueryFilter(t => t.IsActive);
    }
}