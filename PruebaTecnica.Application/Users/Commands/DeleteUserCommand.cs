using MediatR;

namespace PruebaTecnica.Application.Commands;

public record DeleteUserCommand(int Id) : IRequest<bool>;