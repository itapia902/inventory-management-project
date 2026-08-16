namespace ProductService.Domain.Primitives;

public interface IAuditableEntity
{
    DateTime CreatedDateTime { get; set; }
    DateTime? UpdatedDateTime { get; set; }
}