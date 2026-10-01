using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.Handlers.Users;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _userRepository.DeleteAsync(
            request.Id,
            cancellationToken);
    }
}