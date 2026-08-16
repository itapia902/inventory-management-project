using FluentValidation;

namespace ProductService.Application.Product.Commands.UpdateProductStock;

public class UpdateProductStockCommandValidator : AbstractValidator<UpdateProductStockCommand>
{
    public UpdateProductStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("El identificador del producto es obligatorio.");

        RuleFor(x => x.Quantity)
            .NotEqual(0).WithMessage("La cantidad no puede ser cero.");
    }
}