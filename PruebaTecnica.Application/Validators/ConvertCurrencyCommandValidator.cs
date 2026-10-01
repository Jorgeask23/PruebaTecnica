using FluentValidation;
using PruebaTecnica.Application.Commands;

namespace PruebaTecnica.Application.Validators;

public class ConvertCurrencyCommandValidator
    : AbstractValidator<ConvertCurrencyCommand>
{
    public ConvertCurrencyCommandValidator()
    {
        RuleFor(x => x.FromCurrencyCode)
            .Must(IsValidCurrencyCode)
            .WithMessage("El código de moneda de origen debe tener 3 letras.");

        RuleFor(x => x.ToCurrencyCode)
            .Must(IsValidCurrencyCode)
            .WithMessage("El código de moneda de destino debe tener 3 letras.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("El monto debe ser mayor que cero.");
    }

    private static bool IsValidCurrencyCode(string? value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && value.Trim().Length == 3
            && value.Trim().All(char.IsLetter);
    }
}
