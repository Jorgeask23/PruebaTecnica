using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Application.Interfaces;
using PruebaTecnica.Domain.Entities;
using PruebaTecnica.Infrastructure.Data;

namespace PruebaTecnica.Infrastructure.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly AppDbContext _context;

    public CurrencyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Currency>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Currencies
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return await _context.Currencies
            .AnyAsync(
                c => c.Code == normalizedCode,
                cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        int excludingId,
        CancellationToken cancellationToken)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return await _context.Currencies
            .AnyAsync(
                c => c.Code == normalizedCode && c.Id != excludingId,
                cancellationToken);
    }

    public async Task<Currency?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return await _context.Currencies
            .AsNoTracking()
            .FirstOrDefaultAsync(
                currency => currency.Code == normalizedCode,
                cancellationToken);
    }

    public async Task<Currency?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Currencies
            .FirstOrDefaultAsync(
                currency => currency.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Currency currency,
        CancellationToken cancellationToken)
    {
        _context.Currencies.Update(currency);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Currency> CreateAsync(
        Currency currency,
        CancellationToken cancellationToken)
    {
        await _context.Currencies.AddAsync(currency, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return currency;
    }
}
