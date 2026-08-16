using ErrorOr;

namespace ProductService.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static class Product
    {
        private const string Prefix = "Product";

        public static readonly Error ProductNotFound = Error.NotFound(
            $"{Prefix}.NotFound",
            "El producto con el id proporcionado no existe");

        public static readonly Error InvalidQuantity = Error.Validation(
            $"{Prefix}.InvalidQuantity",
            "La cantidad debe ser mayor a cero");

        public static readonly Error ProductUnexpectedError = Error.Unexpected(
            $"{Prefix}.Unexpected",
            "Ha ocurrido un error inesperado, consulte con el Administrador");

        public static Error InsufficientStock(int available, int requested) => Error.Conflict(
            $"{Prefix}.InsufficientStock",
            $"Stock insuficiente. Disponible: {available}, solicitado: {requested}");
    }
}