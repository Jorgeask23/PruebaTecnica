using MediatR;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Commands;

public class CreateCurrencyCommandHandler
    : IRequestHandler<CreateCurrencyCommand, int>
{
    private readonly ICurrencyRepository _currencyRepository;

    public CreateCurrencyCommandHandler(
        ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<int> Handle(
        CreateCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim().ToUpperInvariant();

        var exists = await _currencyRepository.ExistsByCodeAsync(
            code,
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException(
                "Ya existe una moneda con ese código.");
        }

        var currency = new Currency
        {
            Code = code,
            Name = request.Name.Trim(),
            RateToBase = request.RateToBase
        };

        var createdCurrency = await _currencyRepository.CreateAsync(
            currency,
            cancellationToken);

        return createdCurrency.Id;
    }
}
