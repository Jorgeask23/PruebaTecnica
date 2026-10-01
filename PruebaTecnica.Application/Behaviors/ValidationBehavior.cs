using FluentValidation;
using MediatR;

namespace PruebaTecnica.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .SelectMany(v => v.Validate(context).Errors)
                .Where(e => e != null)
                .ToList();

            if (errors.Any())
                throw new ValidationException(errors);
        }

        return await next();
    }
}