using FluentValidation;
using PruebaTecnica.Application.Commands;

namespace PruebaTecnica.Application.Validators;

public class CreateAddressCommandValidator
    : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("El ID del usuario debe ser mayor que cero.");

        RuleFor(x => x.Street)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("La calle es obligatoria.")
            .MaximumLength(150)
            .WithMessage("La calle no puede superar los 150 caracteres.");

        RuleFor(x => x.City)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("La ciudad es obligatoria.")
            .MaximumLength(100)
            .WithMessage("La ciudad no puede superar los 100 caracteres.");

        RuleFor(x => x.Country)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El país es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El país no puede superar los 100 caracteres.");

        RuleFor(x => x.ZipCode)
            .MaximumLength(20)
            .When(x => x.ZipCode is not null)
            .WithMessage("El código postal no puede superar los 20 caracteres.");
    }
}
