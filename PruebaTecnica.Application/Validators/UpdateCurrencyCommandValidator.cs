using FluentValidation;
using PruebaTecnica.Application.Commands;

namespace PruebaTecnica.Application.Validators;

public class UpdateCurrencyCommandValidator
    : AbstractValidator<UpdateCurrencyCommand>
{
    public UpdateCurrencyCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El ID debe ser mayor que cero.");

        RuleFor(x => x.Code)
            .Must(IsValidCurrencyCode)
            .WithMessage("El código debe tener exactamente 3 letras.");

        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El nombre de la moneda es obligatorio.");

        RuleFor(x => x.RateToBase)
            .GreaterThan(0)
            .WithMessage("La tasa de conversión debe ser mayor que cero.");
    }

    private static bool IsValidCurrencyCode(string? value)
    {
        return !string.IsNullOrWhiteSpace(value)
            && value.Trim().Length == 3
            && value.Trim().All(char.IsLetter);
    }
}
