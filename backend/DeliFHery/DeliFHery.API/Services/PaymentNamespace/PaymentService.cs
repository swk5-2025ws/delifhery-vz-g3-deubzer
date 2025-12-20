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
        private readonly IShipmentRepo _shipmentRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly ILabelGenerator _labelGenerator;
        private readonly ICustomerRepo _customerRepo;


        public PaymentService(
            HttpClient httpClient,
            Microsoft.Extensions.Options.IOptions<PaymentOptions> options,
            IPaymentRepo paymentRepo,
            IShipmentRepo shipmentRepo,
            IAddressRepo addressRepo,
            ILabelGenerator labelGenerator,
            ICustomerRepo customerRepo
            )
        {
            _httpClient = httpClient;
            _options = options.Value;
            _paymentRepo = paymentRepo;
            _shipmentRepo = shipmentRepo;
            _addressRepo = addressRepo;
            _labelGenerator = labelGenerator;
            _customerRepo = customerRepo;
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
            var paymentUrl = responseDto.paymentUrl;


            if (!string.IsNullOrWhiteSpace(_options.PublicBaseUrl))
            {
                if (Uri.TryCreate(paymentUrl, UriKind.Absolute, out var paymentUri) &&
                    Uri.TryCreate(_options.PublicBaseUrl, UriKind.Absolute, out var publicBase))
                {
                    var fixedUri = new UriBuilder(paymentUri)
                    {
                        Scheme = publicBase.Scheme,
                        Host = publicBase.Host,
                        Port = publicBase.Port
                    }.Uri;

                    paymentUrl = fixedUri.ToString();
                }
            }
            else
            {
                paymentUrl = paymentUrl.Replace("http://opa:8080", "http://localhost:7172");
            }

            return new PaymentStartResult
            {
                paymentId = paymentId,
                redirectUrl = paymentUrl
            };

        }  
        
        public async Task<PaymentSummaryDto?> GetPaymentSummaryForCustomerAsync(string externalPaymentId,Guid customerId, CancellationToken ct)
        {

            var customer = await _customerRepo.GetByIdAsync(customerId,ct);
            if (customer == null)
            {
                return null;
            }

            var payment = await _paymentRepo.GetByExternalIdAsync(externalPaymentId, ct);
            if(payment == null)
            {
                return null;
            }
            var shipment = await _shipmentRepo.GetByIdAsync(payment.shipmentId, ct);

            if(shipment == null)
            {
                return null;
            }

            var recipientAddress = await _addressRepo.GetAddressByIdAsync(shipment.recipientAddressId,ct );

            if (recipientAddress == null) 
            {
                return null;
            }

            var label = await _labelGenerator.GenerateLabelAsync(shipment.trackingNumber, recipientAddress.name, recipientAddress.street, recipientAddress.postalCode, recipientAddress.city, ct);

            return new PaymentSummaryDto
            {
                paymentId = externalPaymentId,
                status = payment.status ?? "Unknown",
                amount = (decimal)(payment.amount ?? 0),
                currency = payment.currency ?? "EUR",
                trackingNumber = shipment.trackingNumber,
                recipientName = recipientAddress.name,
                recipientStreet = recipientAddress.street,
                recipientPostalCode = recipientAddress.postalCode,
                recipientCity = recipientAddress.city,
                labelImage = label
            };
        }

        public async Task HandleCallbackAsync(PaymentCallbackDto dto, CancellationToken ct)
        {
            if (!string.Equals(dto.apiKey, _options.ApiKey, StringComparison.Ordinal))
            {
                throw new UnauthorizedAccessException("Invalid API key in payment callback.");
            }
                
            var payment = await _paymentRepo.GetByExternalIdAsync(dto.paymentId, ct);
            if (payment == null)
            {
                return;
            }
                
            payment.status = dto.status;
            payment.completedAt = DateTime.UtcNow;

            await _paymentRepo.UpdateStatusAsync(payment, ct);

            if (string.Equals(dto.status, "Success", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(dto.status, "Completed", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(dto.status, "Paid", StringComparison.OrdinalIgnoreCase))
            {
                await _shipmentRepo.UpdateStatusAsync(payment.shipmentId, "Payment completed", ct);
            }
        }
    }
}
