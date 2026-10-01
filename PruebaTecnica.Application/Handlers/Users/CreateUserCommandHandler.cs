using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Commands;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<int> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _userRepository.ExistsByEmailAsync(
            email,
            null,
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException(
                "Ya existe un usuario con ese email.");
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            IsActive = true
        };

        var password = string.IsNullOrWhiteSpace(request.Password)
            ? Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            : request.Password;

        user.Password = _passwordHasher.HashPassword(user, password);

        var createdUser = await _userRepository.CreateAsync(
            user,
            cancellationToken);

        return createdUser.Id;
    }
}
