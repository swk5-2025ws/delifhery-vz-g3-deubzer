using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace DeliFHery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly IShipmentRepo _shipmentRepo;
        private readonly IAddressRepo _addressRepo;
        private readonly ITrackingEventRepo _trackingEventRepo;

        public TrackingController(IShipmentRepo shipmentRepo, IAddressRepo addressRepo, ITrackingEventRepo trackingEventRepo)
        {
            _shipmentRepo = shipmentRepo;
            _addressRepo = addressRepo;
            _trackingEventRepo = trackingEventRepo;
        }

        // GET api/tracking/{postalCode}/{trackingNumber}
        [HttpGet("{postalCode}/{trackingNumber}")]
        public async Task<ActionResult<TrackingStatusResponseDto>> GetTrackingStatus([FromRoute] string trackingNumber,
                                                                                     [FromRoute] string postalCode
                                                                                    , CancellationToken ct)
        {

            if(string.IsNullOrEmpty(trackingNumber) || string.IsNullOrEmpty(postalCode))
            {
                return BadRequest(new
                {
                    message = "Tracking number and Postal code must be added."
                });
            }
            var shipment = await _shipmentRepo.GetShipmentByTrackingNumberAndPostalCode(postalCode.Trim(), trackingNumber.Trim(), ct);

            if(shipment == null)
            {
                return NotFound("No package found.");
            }

            var senderAddress = await _addressRepo.GetAddressByIdAsync(shipment.senderAddressId, ct);
            var recipientAddress = await _addressRepo.GetAddressByIdAsync(shipment.recipientAddressId, ct);

            var events = await _trackingEventRepo.GetByShipmentIdAsync(shipment.shipmentId, ct);

            var response = new TrackingStatusResponseDto
            {
                TrackingNumber = shipment.trackingNumber,
                Sender = senderAddress != null
                ? $"{senderAddress.name}, {senderAddress.postalCode} {senderAddress.city}"
                    : "Unbekannt",
                Recipient = recipientAddress != null
                    ? $"{recipientAddress.name}, {recipientAddress.postalCode} {recipientAddress.city}"
                    : "Unbekannt",
                History = events
                .OrderBy(e => e.occurredAt)
                    .Select(e => new TrackingStatusEventDto
                    {
                        OccurredAt = e.occurredAt,
                        Status = e.status,
                        Location = e.location,
                        Note = e.note
                    })
                    .ToList()
            };

            return Ok(response);
        }
    }
}
