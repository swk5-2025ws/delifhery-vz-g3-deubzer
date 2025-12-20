using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;

namespace DeliFHery.API.Services
{
    public class CarrierTrackingService : ICarrierTrackingService
    {
        private readonly ICarrierAuthRepo _authRepo;
        private readonly IShipmentRepo _shipmentRepo;
        private readonly ITrackingEventRepo _trackingEventRepo;
        private readonly IContactMethodRepo _contactMethodRepo;
        private readonly INotificationSubscriptionRepo _notificationSubscriptionRepo;
        private readonly IEmailSender _emailSender;

        public CarrierTrackingService(

            ICarrierAuthRepo authRepo,
            IShipmentRepo shipmentRepo,
            ITrackingEventRepo trackingEventRepo,
            IContactMethodRepo contactMethodRepo,
            INotificationSubscriptionRepo notificationSubscriptionRepo,
            IEmailSender emailSender

        )
        {
            _authRepo = authRepo;
            _shipmentRepo = shipmentRepo;
            _trackingEventRepo = trackingEventRepo;
            _contactMethodRepo = contactMethodRepo;
            _notificationSubscriptionRepo = notificationSubscriptionRepo;
            _emailSender = emailSender;
        }

        public async Task UpdateStatusAsync(string apiKey, TrackingStatusUpdateDto dto, CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace( apiKey ) || !await _authRepo.IsValidAPIKeyAsync(apiKey, ct))
            {
                throw new UnauthorizedAccessException("Invalid Api key");
            }

            if (string.IsNullOrWhiteSpace(dto.TrackingNumber))
            {
                throw new ArgumentException("Tracking Number is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                throw new ArgumentException("Status is required.");
            }

            var shipment = await _shipmentRepo.GetShipmentByTrackingNumber(dto.TrackingNumber.Trim(), ct);
            if (shipment == null)
            {
                throw new KeyNotFoundException("Shipment not found");
            }


            var trackingEvent = new TrackingEvent
            {
                shipmentId = shipment.shipmentId,
                status = dto.Status,
                location = dto.Zusatzinformation,
                note = dto.Note,
                occurredAt = DateTime.UtcNow
            };

            await _trackingEventRepo.CreateAsync(trackingEvent, ct);

            await _shipmentRepo.UpdateStatusAsync(shipment.shipmentId, dto.Status, ct);
            Console.WriteLine("updatetStatus");
            await NotifySubscriptionAsync(shipment, trackingEvent, ct);
        }

        private async Task NotifySubscriptionAsync(Shipment shipment, TrackingEvent trackingEvent, CancellationToken ct)
        {
            var customerIds = await _notificationSubscriptionRepo
                .GetSubscribedCustomerIdAsync(shipment.shipmentId, ct);

            if (customerIds.Count == 0) return;

            var tasks = customerIds.Select(async customerId =>
            {
                try
                {
                    var email = await _contactMethodRepo.GetPrimaryEmailAsny(customerId, ct);
                    if (string.IsNullOrWhiteSpace(email)) return;

                    var subject = $"Tracking update: {shipment.trackingNumber}";
                    var body =
        $@"Shipment update

Tracking: {shipment.trackingNumber}
Status: {trackingEvent.status}
Location: {trackingEvent.location ?? "-"}
Note: {trackingEvent.note ?? "-"}
Time: {trackingEvent.occurredAt:yyyy-MM-dd HH:mm:ss}
";

                    await _emailSender.SendAsync(email, subject, body, ct);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"failed to send email to {customerId}: {ex.Message}");
                }
            });

            await Task.WhenAll(tasks);
        }

    }
}
