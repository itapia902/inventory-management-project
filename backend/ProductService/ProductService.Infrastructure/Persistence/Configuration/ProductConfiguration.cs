using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Domain.Product;
using ProductService.Domain.Product.ValueObjects;

namespace ProductService.Infrastructure.Persistence.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Producto");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(v => v.Value, v => ProductId.CreateUnique(v))
            .ValueGeneratedNever();    
        
        builder.Property(p => p.Name)
             .HasMaxLength(150)
             .IsRequired()
             .HasColumnName("Nombre");

        builder.Property(p => p.Category)
            .HasMaxLength(100)
            .IsRequired()
            .HasColumnName("Categoria");

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .IsRequired()
            .HasColumnName("Descripcion");

        builder.Property(p => p.Price)
              .IsRequired()
              .HasColumnType("decimal(18, 2)")
              .HasColumnName("Precio");

        builder.Property(p => p.Stock)
            .IsRequired()
            .HasColumnName("Stock");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasColumnName("Activo");    

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500)            
            .HasColumnName("UrlImagen");

        builder.Property(p => p.CreatedDateTime)
            .IsRequired()
            .HasColumnName("FechaCreacion");

        builder.Property(p => p.UpdatedDateTime)
            .HasColumnName("FechaModificacion");

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Category);


    }
}