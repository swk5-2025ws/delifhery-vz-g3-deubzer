using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface ICustomerRepo
    {
        Task<int> CreateAsync(Customer c, CancellationToken ct = default);
        Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken ct = default);
        Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IList<Customer>> FindByUsernameAsync(string username, CancellationToken ct = default);
        Task<bool> UpdateUsernameAsync(int id, string username, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
