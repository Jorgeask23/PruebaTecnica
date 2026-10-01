using MediatR;
using PruebaTecnica.Application.DTOs;

namespace PruebaTecnica.Application.Queries;

public record GetAddressesQuery(int UserId) : IRequest<List<AddressResponse>>;