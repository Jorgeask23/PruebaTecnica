using MediatR;

namespace PruebaTecnica.Application.Commands;

public record CreateCurrencyCommand(
    string Code,
    string Name,
    decimal RateToBase
) : IRequest<int>;