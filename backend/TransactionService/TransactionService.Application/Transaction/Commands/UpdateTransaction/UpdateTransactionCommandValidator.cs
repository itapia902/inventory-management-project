using FluentValidation;

namespace TransactionService.Application.Transaction.Commands.UpdateTransaction;
public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El identificador de la transacción es obligatorio.");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("La fecha de la transacción es obligatoria.")
            .LessThan(_ => DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("La fecha de la transacción no puede ser futura.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("El precio unitario no puede ser negativo.");

        RuleFor(x => x.Detail)
            .MaximumLength(500).WithMessage("El detalle no puede exceder 500 caracteres.");
    }
}