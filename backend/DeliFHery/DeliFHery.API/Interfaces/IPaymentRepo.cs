using DeliFHery.API.Models;

namespace DeliFHery.API.Interfaces
{
    public interface IPaymentRepo
    {
        Task<int> CreateAsync(Payment payment, CancellationToken ct);
        Task<Payment?> GetByIdAsync(int paymentId, CancellationToken ct);
        Task<Payment?> GetByExternalIdAsync(string externalId, CancellationToken ct);
        Task UpdateStatusAsync(int paymentId, string status,DateTime? completedAt, CancellationToken ct);
    }
}
