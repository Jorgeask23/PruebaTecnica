using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Handlers.Currencies;

public class UpdateCurrencyCommandHandler
    : IRequestHandler<UpdateCurrencyCommand, Unit>
{
    private readonly ICurrencyRepository _currencyRepository;

    public UpdateCurrencyCommandHandler(
        ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<Unit> Handle(
        UpdateCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var currency = await _currencyRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (currency is null)
            throw new KeyNotFoundException("Moneda no encontrada.");

        var code = request.Code.Trim().ToUpperInvariant();

        var codeExists = await _currencyRepository.ExistsByCodeAsync(
            code,
            request.Id,
            cancellationToken);

        if (codeExists)
        {
            throw new InvalidOperationException(
                "Ya existe una moneda con ese código.");
        }

        currency.Code = code;
        currency.Name = request.Name.Trim();
        currency.RateToBase = request.RateToBase;

        await _currencyRepository.UpdateAsync(
            currency,
            cancellationToken);

        return Unit.Value;
    }
}