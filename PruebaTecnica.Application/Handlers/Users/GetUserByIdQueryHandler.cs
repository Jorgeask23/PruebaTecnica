using MediatR;
using PruebaTecnica.Application.DTOs;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Application.Queries;

namespace PruebaTecnica.Application.Handlers.Users;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse?> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (user is null)
            return null;

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            IsActive = user.IsActive
        };
    }
}