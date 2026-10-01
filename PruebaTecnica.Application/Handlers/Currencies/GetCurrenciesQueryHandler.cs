using MediatR;
using PruebaTecnica.Application.DTOs;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.Queries;

public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, List<CurrencyResponse>>
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<List<CurrencyResponse>> Handle(
        GetCurrenciesQuery request,
        CancellationToken cancellationToken)
    {
        var currencies = await _currencyRepository.GetAllAsync(cancellationToken);

        return currencies.Select(currency => new CurrencyResponse
        {
            Id = currency.Id,
            Code = currency.Code,
            Name = currency.Name,
            RateToBase = currency.RateToBase
        }).ToList();
    }
}