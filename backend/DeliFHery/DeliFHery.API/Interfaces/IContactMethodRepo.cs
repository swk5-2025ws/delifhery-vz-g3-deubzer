using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IContactMethodRepo
    {
        Task<int> CreateAsync(ContactMethod contactMethod, CancellationToken ct = default);
        Task<IList<ContactMethod>> ListForCustomer(int customerId, CancellationToken ct = default);
        Task<bool> UpdateAsync(ContactMethod contactMethod,CancellationToken ct = default);
        Task<bool> DeleteAsync(int contactId, CancellationToken ct = default);
    }
}
