using FluentValidation;
using PruebaTecnica.Application.Commands;

namespace PruebaTecnica.Application.Validators;

public class UpdateAddressCommandValidator
    : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El ID debe ser mayor que cero.");

        RuleFor(x => x.Street)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("La calle es obligatoria.");

        RuleFor(x => x.City)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("La ciudad es obligatoria.");

        RuleFor(x => x.Country)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El país es obligatorio.");

        RuleFor(x => x.ZipCode)
            .MaximumLength(20)
            .When(x => x.ZipCode is not null)
            .WithMessage("El código postal no puede superar los 20 caracteres.");
    }
}
