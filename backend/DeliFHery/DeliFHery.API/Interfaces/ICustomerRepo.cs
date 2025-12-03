using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface ICustomerRepo
    {
        Task<Guid> CreateAsync(Customer c, CancellationToken ct = default);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken ct = default);
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Customer?> GetByIdentityProviderUserIdAsync(string idntityUserId, CancellationToken ct = default);
        Task<IList<Customer>> FindByUsernameAsync(string username, CancellationToken ct = default);
        Task<bool> UpdateUsernameAsync(Guid id, string username, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
