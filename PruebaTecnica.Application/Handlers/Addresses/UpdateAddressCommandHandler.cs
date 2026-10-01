using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.Handlers.Addresses;

public class UpdateAddressCommandHandler
    : IRequestHandler<UpdateAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;

    public UpdateAddressCommandHandler(
        IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<bool> Handle(
        UpdateAddressCommand request,
        CancellationToken cancellationToken)
    {
        var address = await _addressRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (address is null)
        {
            return false;
        }

        address.Street = request.Street.Trim();
        address.City = request.City.Trim();
        address.Country = request.Country.Trim();
        address.ZipCode = string.IsNullOrWhiteSpace(request.ZipCode)
            ? null
            : request.ZipCode.Trim();

        return await _addressRepository.UpdateAsync(
            address,
            cancellationToken);
    }
}
