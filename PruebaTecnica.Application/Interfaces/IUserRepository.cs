using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Interfaces;

public interface IUserRepository
{
    Task<User> CreateAsync(
        User user,
        CancellationToken cancellationToken);

    Task<bool> ExistsByEmailAsync(
        string email,
        int? excludingUserId,
        CancellationToken cancellationToken);

    Task<List<User>> GetAllAsync(
        bool? isActive,
        CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        User user,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken);
}
