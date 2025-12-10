using DeliFHery.API.Dto;

namespace DeliFHery.API.Interfaces
{
    public class PaymentStartResult
    {
        public int paymentId {  get; set; }
        public string redirectUrl { get; set; } = default!;
    }
    public interface IPaymentService
    {
        Task<PaymentStartResult> StartPaymentAsync(int shipmentId, decimal amount, string currency, CancellationToken ct);
        Task<PaymentSummaryDto?> GetPaymentSummaryAsync(string externalPaymentId, CancellationToken ct);
    }
}
