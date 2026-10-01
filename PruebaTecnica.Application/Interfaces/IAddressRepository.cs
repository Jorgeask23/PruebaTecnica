using PruebaTecnica.Domain.Entities;

namespace PruebaTecnica.Application.Interfaces;

public interface IAddressRepository
{
    Task<List<Address>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken);

    Task<Address> CreateAsync(
        Address address,
        CancellationToken cancellationToken);

    Task<Address?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<bool> UpdateAsync(
        Address address,
        CancellationToken cancellationToken);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken);
}