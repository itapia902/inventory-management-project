using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionService.Domain.Transaction.ValueObjects;
using TransactionService.Domain.Transaction;

namespace TransactionService.Infrastructure.Persistence.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaccion");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(v => v.Value, v => TransactionId.CreateUnique(v))
            .ValueGeneratedNever();

        builder.Property(t => t.ProductId)
            .HasConversion(v => v.Value, v => ProductId.CreateUnique(v))
            .IsRequired()
            .HasColumnName("ProductoId");

        builder.Property(t => t.TransactionDate)
            .IsRequired()
            .HasColumnName("Fecha");

        builder.Property(t => t.Type)
            .HasConversion<byte>()
            .IsRequired()
            .HasColumnName("Tipo");

        builder.Property(t => t.Quantity)
            .IsRequired()
            .HasColumnName("Cantidad");

        builder.Property(t => t.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("PrecioUnitario");

        builder.Property(t => t.TotalPrice)
            .IsRequired()
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("PrecioTotal");

        builder.Property(t => t.Detail)
            .HasMaxLength(500)
            .HasColumnName("Detalle");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasColumnName("Activo");

        builder.Property(t => t.CreatedDateTime)
            .IsRequired()
            .HasColumnName("FechaCreacion");

        builder.Property(t => t.UpdatedDateTime)
            .HasColumnName("FechaModificacion");

        builder.HasIndex(t => t.ProductId);
        builder.HasIndex(t => t.TransactionDate);
        builder.HasIndex(t => t.Type);

    }
}