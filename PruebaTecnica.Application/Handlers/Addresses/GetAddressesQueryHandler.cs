using MediatR;
using PruebaTecnica.Application.DTOs;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.Queries;

public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, List<AddressResponse>>
{
    private readonly IAddressRepository _addressRepository;

    public GetAddressesQueryHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<List<AddressResponse>> Handle(
        GetAddressesQuery request,
        CancellationToken cancellationToken)
    {
        var addresses = await _addressRepository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        return addresses.Select(address => new AddressResponse
        {
            Id = address.Id,
            UserId = address.UserId,
            Street = address.Street,
            City = address.City,
            Country = address.Country,
            ZipCode = address.ZipCode
        }).ToList();
    }
}