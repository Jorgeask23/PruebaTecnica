using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.CurrencyConversion;

public class ConvertCurrencyCommandHandler
    : IRequestHandler<ConvertCurrencyCommand, decimal>
{
    private readonly ICurrencyRepository _currencyRepository;

    public ConvertCurrencyCommandHandler(
        ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<decimal> Handle(
        ConvertCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var fromCode = request.FromCurrencyCode.Trim().ToUpperInvariant();
        var toCode = request.ToCurrencyCode.Trim().ToUpperInvariant();

        var fromCurrency = await _currencyRepository.GetByCodeAsync(
            fromCode,
            cancellationToken);

        var toCurrency = await _currencyRepository.GetByCodeAsync(
            toCode,
            cancellationToken);

        if (fromCurrency is null || toCurrency is null)
        {
            throw new InvalidOperationException(
                "Una o ambas monedas no existen.");
        }

        var amountInBase = request.Amount * fromCurrency.RateToBase;
        var convertedAmount = amountInBase / toCurrency.RateToBase;

        return Math.Round(convertedAmount, 2);
    }
}
