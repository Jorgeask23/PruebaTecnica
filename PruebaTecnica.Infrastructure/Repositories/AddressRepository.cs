using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Data;

namespace PruebaTecnica.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Address>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        return await _context.Addresses
            .AsNoTracking()
            .Where(address => address.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Address> CreateAsync(
        Address address,
        CancellationToken cancellationToken)
    {
        await _context.Addresses.AddAsync(address, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return address;
    }

    public async Task<Address?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(address => address.Id == id, cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        Address address,
        CancellationToken cancellationToken)
    {
        _context.Addresses.Update(address);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(address => address.Id == id, cancellationToken);

        if (address is null)
            return false;

        _context.Addresses.Remove(address);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}