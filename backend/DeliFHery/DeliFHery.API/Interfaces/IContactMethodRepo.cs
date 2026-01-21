using DeliFHery.API.Models;
using YamlDotNet.Serialization.NamingConventions;

namespace DeliFHery.API.Interfaces
{
    public interface IContactMethodRepo
    {
        Task<int> CreateAsync(ContactMethod contactMethod, CancellationToken ct = default);
        Task<IEnumerable<ContactMethod>> ListForCustomerAsync(Guid customerId, CancellationToken ct = default);
        Task<int> ClearPrimaryAsync(Guid customerId,CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid customerId,int contactId, CancellationToken ct = default);
        Task<string?> GetPrimaryEmailAsny(Guid customerId, CancellationToken ct);
        Task<bool> CheckPrimary(Guid customerId, CancellationToken ct);
        Task<bool> SetPrimaryAsync(Guid customerId, int contactId, CancellationToken ct);
    }
}
