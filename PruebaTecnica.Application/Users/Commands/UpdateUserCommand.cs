using MediatR;

namespace PruebaTecnica.Application.Commands;

public record UpdateUserCommand(
    int Id,
    string Name,
    string Email,
    bool IsActive
) : IRequest<bool>;