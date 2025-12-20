using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface ICarrierRepo
    {
        Task<int> CreateAsync(Carrier carrier, CancellationToken ct);
    }
}
