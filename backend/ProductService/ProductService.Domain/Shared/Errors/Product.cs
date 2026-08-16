using ErrorOr;

namespace ProductService.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Product
    {        
        public static readonly Error ProductNotFound = Error.NotFound("Product.NotFound", "El producto con el id proporcionado no existe");

        public static readonly Error InvalidQuantity = Error.Validation("Product.Validation", "La cantidad debe ser mayor a cero");

        public static readonly Error ProductUnexpectedError = Error.Unexpected("Product.Unexpected", "Ha ocurrido un error inesperado, consulte con el Administrador");
        public static Error InsufficientStock(int available, int requested) => Error.Conflict("Product.Conflict", $"Stock insuficiente. Disponible: {available}, solicitado: {requested}");
    }
}