using MediatR;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Commands;

public class CreateAddressCommandHandler
    : IRequestHandler<CreateAddressCommand, int>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;

    public CreateAddressCommandHandler(
        IAddressRepository addressRepository,
        IUserRepository userRepository)
    {
        _addressRepository = addressRepository;
        _userRepository = userRepository;
    }

    public async Task<int> Handle(
        CreateAddressCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("Usuario no encontrado.");
        }

        var address = new Address
        {
            UserId = request.UserId,
            Street = request.Street.Trim(),
            City = request.City.Trim(),
            Country = request.Country.Trim(),
            ZipCode = string.IsNullOrWhiteSpace(request.ZipCode)
                ? null
                : request.ZipCode.Trim()
        };

        var createdAddress = await _addressRepository.CreateAsync(
            address,
            cancellationToken);

        return createdAddress.Id;
    }
}
