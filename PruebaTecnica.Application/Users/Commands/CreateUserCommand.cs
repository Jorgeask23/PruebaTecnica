using MediatR;

namespace PruebaTecnica.Application.Commands;

public record CreateUserCommand(
    string Name,
    string Email,
    string? Password = null
) : IRequest<int>;
