using DeliFHery.API.Interfaces;
using System.Net.WebSockets;

namespace DeliFHery.API.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        public Task<PaymentStartResult> StartPaymentAsync(int shipmentId, decimal amount, string currency, CancellationToken ct)
        {
           var externalId =  Guid.NewGuid().ToString("N");
           var redirectUrl = $"https://dummy-payment.local/{externalId}";

            return Task.FromResult(new PaymentStartResult
            {
                paymentId = 0,
                redirectUrl = redirectUrl,
            });

        }  
    }
}
