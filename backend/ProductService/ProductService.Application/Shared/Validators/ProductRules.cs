using FluentValidation;

namespace ProductService.Application.Shared.Validators;

public static class ProductRules
{
    public static IRuleBuilderOptions<T, string> ValidProductName<T>(
        this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres.");

    public static IRuleBuilderOptions<T, string> ValidDescription<T>(
        this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres.");

    public static IRuleBuilderOptions<T, string> ValidCategory<T>(
        this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("La categoría es obligatoria.")
            .MaximumLength(100).WithMessage("La categoría no puede exceder 100 caracteres.");

    public static IRuleBuilderOptions<T, decimal> ValidPrice<T>(
        this IRuleBuilder<T, decimal> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");

    public static IRuleBuilderOptions<T, string?> ValidImageUrl<T>(
        this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder
            .Must(BeAValidUrl).WithMessage("La URL de la imagen no es válida.");

    private static bool BeAValidUrl(string? url) =>
        string.IsNullOrWhiteSpace(url) ||
        (Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
         (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps));
}