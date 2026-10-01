using MediatR;

namespace PruebaTecnica.Application.Commands;

public record UpdateAddressCommand(
    int Id,
    string Street,
    string City,
    string Country,
    string? ZipCode
) : IRequest<bool>; 