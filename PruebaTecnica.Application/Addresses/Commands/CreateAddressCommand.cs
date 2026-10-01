using MediatR;

namespace PruebaTecnica.Application.Commands;

public record CreateAddressCommand(
    int UserId,
    string Street,
    string City,
    string Country,
    string? ZipCode
) : IRequest<int>;