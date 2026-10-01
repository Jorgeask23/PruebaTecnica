using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.Handlers.Addresses;

public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, bool>
{
    private readonly IAddressRepository _addressRepository;

    public DeleteAddressCommandHandler(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<bool> Handle(
        DeleteAddressCommand request,
        CancellationToken cancellationToken)
    {
        return await _addressRepository.DeleteAsync(
            request.Id,
            cancellationToken);
    }
}