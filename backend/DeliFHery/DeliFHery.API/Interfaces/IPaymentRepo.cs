using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IPaymentRepo
    {
        public Task<int> CreateAsync(Payment payment, CancellationToken ct);
        public Task<Payment?> GetByIdAsync(int paymentId, CancellationToken ct);
        public Task<Payment?> GetByExternalIdAsync(string externalId, CancellationToken ct);
        public Task UpdateStatusAsync(Payment payment, CancellationToken ct);
    }
}
