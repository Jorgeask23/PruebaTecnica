using MediatR;
using PruebaTecnica.Application.DTOs;
using PruebaTecnica.Application.Interfaces;

namespace PruebaTecnica.Application.Queries;

public class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, List<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserResponse>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(
            request.IsActive,
            cancellationToken);

        return users.Select(user => new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            IsActive = user.IsActive
        }).ToList();
    }
}
