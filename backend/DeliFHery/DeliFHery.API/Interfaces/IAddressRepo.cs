using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IAddressRepo
    {
        Task<int> CreateAsync(Address address,CancellationToken ct = default);
        Task<bool> UpdateAsync(Address address,CancellationToken ct = default);
        Task<Address?> GetAddressByIdAsync(int id,CancellationToken ct = default);
        Task<bool> DeleteAsync(int id,CancellationToken ct = default);
    }
}
