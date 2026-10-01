using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Interfaces;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Currency> CreateAsync(
        Currency currency,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        string code,
        int excludingId,
        CancellationToken cancellationToken);

    Task<Currency?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken);

    Task<Currency?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Currency currency,
        CancellationToken cancellationToken);
}
