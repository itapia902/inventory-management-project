using FluentValidation;
using ProductService.Application.Shared.Validators;

namespace ProductService.Application.Product.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El identificador del producto es obligatorio.");

        RuleFor(x => x.Name).ValidProductName();
        RuleFor(x => x.Description).ValidDescription();
        RuleFor(x => x.Category).ValidCategory();
        RuleFor(x => x.Price).ValidPrice();
        RuleFor(x => x.ImageUrl).ValidImageUrl();
    }
}