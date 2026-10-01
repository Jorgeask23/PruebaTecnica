using MediatR;

namespace PruebaTecnica.Application.Commands;

public record UpdateCurrencyCommand(
    int Id,
    string Code,
    string Name,
    decimal RateToBase
) : IRequest<Unit>;