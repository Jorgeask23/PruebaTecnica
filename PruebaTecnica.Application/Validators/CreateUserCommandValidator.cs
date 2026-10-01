using FluentValidation;
using PruebaTecnica.Application.Commands;

namespace PruebaTecnica.Application.Validators;

public class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Email)
            .Must(value => !string.IsNullOrWhiteSpace(value))
            .WithMessage("El email es obligatorio.")
            .EmailAddress()
            .WithMessage("El formato del email no es válido.");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .When(x => !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage("La contraseña debe tener al menos 8 caracteres.");
    }
}
