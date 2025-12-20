using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IContactMethodRepo
    {
        Task<int> CreateAsync(ContactMethod contactMethod, CancellationToken ct = default);
        Task<IEnumerable<ContactMethod>> ListForCustomerAsync(Guid customerId, CancellationToken ct = default);
        Task<bool> UpdateAsync(ContactMethod contactMethod,CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid customerId,int contactId, CancellationToken ct = default);
        Task<string?> GetPrimaryEmailAsny(Guid customerId, CancellationToken ct);
    }
}
