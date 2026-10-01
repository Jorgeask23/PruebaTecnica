using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Data;

namespace PruebaTecnica.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(
        User user,
        CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        int? excludingUserId,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        var query = _context.Users
            .AsNoTracking()
            .Where(x => x.Email == normalizedEmail);

        if (excludingUserId.HasValue)
        {
            query = query.Where(x => x.Id != excludingUserId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(
        bool? isActive,
        CancellationToken cancellationToken)
    {
        var query = _context.Users
            .AsNoTracking()
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        User user,
        CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == user.Id,
                cancellationToken);

        if (existingUser is null)
        {
            return false;
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.IsActive = user.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (user is null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
