using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using System.Net;
using System.Net.WebSockets;

namespace DeliFHery.API.Services.PaymentNamespace
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly PaymentOptions _options;
        private readonly IPaymentRepo _paymentRepo;


        public PaymentService(
            HttpClient httpClient,
            Microsoft.Extensions.Options.IOptions<PaymentOptions> options,
            IPaymentRepo paymentRepo
            )
        {
            _httpClient = httpClient;
            _options = options.Value;
            _paymentRepo = paymentRepo;
        }


        public async Task<PaymentStartResult> StartPaymentAsync(int shipmentId, decimal amount, string currency, CancellationToken ct)
        {
           var externalPaymentId =  Guid.NewGuid().ToString("N");

            var request = new PaymentStartRequestDto
            {
                paymentId = externalPaymentId,
                amount = amount,
                callbackUrl = _options.CallbackUrl,
                redirectUrl = _options.RedirectUrl,
            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, _options.StartUrl)
            {
                Content = JsonContent.Create(request)
            };
            httpRequest.Headers.Add("X-API-Key", _options.ApiKey);

            using var response = await _httpClient.SendAsync(httpRequest, ct);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var conflictText = await response.Content.ReadAsStringAsync(ct);
                throw new InvalidOperationException(
                    $"OPA payment conflict for id {externalPaymentId}: {conflictText}");
            }

            response.EnsureSuccessStatusCode();

            var responseDto = await response.Content
                .ReadFromJsonAsync<PaymentStartResponseDto>(cancellationToken: ct);

            if (responseDto == null || string.IsNullOrWhiteSpace(responseDto.paymentUrl ))
            {
                throw new InvalidOperationException("OPA did not return a paymentUrl.");
            }

            var payment = new Payment
            {
                shipmentId = shipmentId,
                externalPaymentId = externalPaymentId,
                amount = (double) amount,
                currency = currency,
                status = "Pending",
                callBackUrl = _options.CallbackUrl,
                redirectUrl = _options.RedirectUrl,
                createdAt = DateTime.UtcNow
            };

            var paymentId = await _paymentRepo.CreateAsync(payment, ct);

            return new PaymentStartResult
            {
                paymentId = paymentId,
                redirectUrl = responseDto.paymentUrl
            };

        }  
    }
}
