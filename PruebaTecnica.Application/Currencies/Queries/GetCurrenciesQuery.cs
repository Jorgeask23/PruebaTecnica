using MediatR;
using PruebaTecnica.Application.DTOs;

namespace PruebaTecnica.Application.Queries;

public record GetCurrenciesQuery() : IRequest<List<CurrencyResponse>>;