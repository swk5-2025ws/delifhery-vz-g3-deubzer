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

        public CarrierTrackingService(

            ICarrierAuthRepo authRepo,
            IShipmentRepo shipmentRepo,
            ITrackingEventRepo trackingEventRepo
        )
        {
            _authRepo = authRepo;
            _shipmentRepo = shipmentRepo;
            _trackingEventRepo = trackingEventRepo;
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

            var shipment = await _shipmentRepo.GetShipmentByTrackingNumber(dto.TrackingNumber, ct);
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
        }
    }
}
