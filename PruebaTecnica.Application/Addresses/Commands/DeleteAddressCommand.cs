using MediatR;

namespace PruebaTecnica.Application.Commands;

public record DeleteAddressCommand(int Id) : IRequest<bool>;