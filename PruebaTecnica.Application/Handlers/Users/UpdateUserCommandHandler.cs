using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.Handlers.Users;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (user is null)
        {
            return false;
        }

        var email = request.Email.Trim().ToLowerInvariant();

        var emailExists = await _userRepository.ExistsByEmailAsync(
            email,
            request.Id,
            cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "Ya existe un usuario con ese email.");
        }

        user.Name = request.Name.Trim();
        user.Email = email;
        user.IsActive = request.IsActive;

        return await _userRepository.UpdateAsync(
            user,
            cancellationToken);
    }
}
