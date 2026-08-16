using FluentValidation;
using ProductService.Application.Shared.Validators;

namespace ProductService.Application.Product.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).ValidProductName();
        RuleFor(x => x.Description).ValidDescription();
        RuleFor(x => x.Category).ValidCategory();
        RuleFor(x => x.Price).ValidPrice();
        RuleFor(x => x.ImageUrl).ValidImageUrl();

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
    }
}