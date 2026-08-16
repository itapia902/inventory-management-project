using ErrorOr;

public static partial class DomainErrors
{
    public static class Transaction
    {
        public static readonly Error TransactionNotFound = Error.NotFound("Transaction.NotFound", "La transacción con el id proporcionado no existe");

        public static readonly Error TransactionUnexpectedError = Error.Unexpected("Transaction.Unexpected", "Ha ocurrido un error inesperado, consulte con el administrador");

        public static readonly Error ProductNotFound = Error.NotFound("Transaction.ProductNotFound", "El producto indicado no existe");

        public static readonly Error ProductServiceUnavailable = Error.Unexpected("Transaction.ProductServiceUnavailable", "No se pudo comunicar con el servicio de productos. Intente nuevamente");
    }
}