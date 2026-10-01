using FluentValidation;
using PruebaTecnica.Application.Commands;

namespace PruebaTecnica.Application.Validators;

public class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El ID debe ser mayor que cero.");

        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Email)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage("El correo electrónico no tiene un formato válido.");
    }
}
