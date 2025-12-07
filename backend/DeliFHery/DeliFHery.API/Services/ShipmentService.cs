using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using System.Security.Cryptography;

namespace DeliFHery.API.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepo _shipmentRepo;
        private readonly IShipmentPriceRepo _shipmentPriceRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly IShippingPriceCalculator _shippingPriceCalculator;
        private readonly IPaymentService _paymentService;
        private readonly ILabelGenerator _labelGenerator;

        public ShipmentService(IShipmentRepo shipmentRepo, 
                               IShipmentPriceRepo shipmentPriceRepo, 
                               IAddressRepo addressRepo,
                               IShippingPriceCalculator shippingPriceCalculator, 
                               IPaymentService paymentService, 
                               ILabelGenerator labelGenerator)
        {
            _shipmentRepo = shipmentRepo;
            _shipmentPriceRepo = shipmentPriceRepo;
            _addressRepo = addressRepo;
            _shippingPriceCalculator = shippingPriceCalculator;
            _paymentService = paymentService;
            _labelGenerator = labelGenerator;
        }


        private async Task<string> GenerateUniqueTrackingNumberAsync(CancellationToken ct)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            const int length = 11;

            while (true)
            {
                var bytes = RandomNumberGenerator.GetBytes(length);
                var span = new char[length];

                for (var i = 0; i < length; i++)
                {
                    span[i] = chars[bytes[i] % chars.Length];
                }

                var candidate = new string(span);

                var existing = await _shipmentRepo.GetShipmentByTrackingNumber(candidate, ct);
                if (existing == null)
                    return candidate;
            }
        }


        public async Task<CreateShipmentResponseDto> CreateShipmentAsync(CreateShipmentRequestDto request, 
                                                                   Guid senderCustomerId,
                                                                   CancellationToken ct)
        {
            var senderAddress = new Address
            {
                name = request.SenderName,
                street = request.SenderStreet,
                city = request.SenderCity,
                postalCode = request.SenderPostalCode,
                houseNumber = request.SenderHouseNumber,
            };

            var recipientAddress = new Address 
            {
                name = request.RecipientName,
                street = request.RecipientStreet,
                city = request.RecipientCity,
                postalCode = request.RecipientPostalCode,
                houseNumber = request.RecipientHouseNumber,
            };

            var senderAddressId = await _addressRepo.CreateAsync(senderAddress,ct);
            var recipientAddressId = await _addressRepo.CreateAsync(recipientAddress,ct);

            var priceResult = await _shippingPriceCalculator.CalculatePriceAsync(
                new CalculateShipmentPriceRequestDto
                {
                    SenderPostalCode = request.SenderPostalCode,
                    SenderCity = request.SenderCity,
                    SenderStreet = request.SenderStreet,
                    SenderHouseNumber = request.SenderHouseNumber,
                    RecipientPostalCode = request.RecipientPostalCode,
                    RecipientCity = request.RecipientCity,
                    RecipientStreet = request.RecipientStreet,
                    RecipientHouseNumber = request.RecipientHouseNumber,
                    WidthCm = request.WidthCm,
                    HeightCm = request.HeightCm,
                    LengthCm = request.LengthCm,
                    WeightKg = request.WeightKg
                });

            var totalPrice = priceResult.TotalPrice;

            var trackingNumber = await GenerateUniqueTrackingNumberAsync(ct);

            var shipment = new Shipment
            {
                senderCustomerId = senderCustomerId,
                senderAddressId = senderAddressId,
                recipientAddressId = recipientAddressId,
                trackingNumber = trackingNumber,
                weightKg = request.WeightKg,
                heightCm = request.HeightCm,
                widthCm = request.WidthCm,
                lengthCm = request.LengthCm,
                currentStatus = "PendingPayment",
                createdAt = DateTime.UtcNow
            };

            var shipmentId = await _shipmentRepo.CreateAsync(shipment, ct);

            await _shipmentPriceRepo.CreateAsync(new ShipmentPrice
            {
                shipmentId = shipmentId,
                amount = (double)totalPrice,
                currency = priceResult.Currency,
                calculatedAt = DateTime.UtcNow
            }, ct);

            var paymentResult = await _paymentService.StartPaymentAsync(shipmentId, totalPrice, priceResult.Currency, ct);

            var labelBase = await _labelGenerator.GenerateLabelAsync(
                trackingNumber,
                request.RecipientName,
                request.RecipientStreet,
                request.RecipientHouseNumber,
                request.RecipientPostalCode,
                request.RecipientCity,
                ct);


            return new CreateShipmentResponseDto
            {
                TrackingNumber = trackingNumber,
                Price = totalPrice,
                Currency = priceResult.Currency,
                PaymentUrl = paymentResult.redirectUrl,
                LabelImage = labelBase
            };
        }
    }
}
