using MediatR;

namespace PruebaTecnica.Application.Commands;

public record ConvertCurrencyCommand(
    string FromCurrencyCode,
    string ToCurrencyCode,
    decimal Amount
) : IRequest<decimal>;
