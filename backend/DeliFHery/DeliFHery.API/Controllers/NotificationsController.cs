using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliFHery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IShipmentRepo _shipmentRepo;
        private readonly INotificationSubscriptionRepo _notificationSubscriptionRepo;

        public NotificationsController(
            ICustomerRepo customerRepo,
            IShipmentRepo shipmentRepo,
            INotificationSubscriptionRepo notificationSubscriptionRepo)
        {
            _customerRepo = customerRepo;
            _shipmentRepo = shipmentRepo;
            _notificationSubscriptionRepo = notificationSubscriptionRepo;
        }

        // GET api/notifications/subscription/{postalCode}/{trackingNumber}

        [Authorize]
        [HttpGet("subscription/{postalCode}/{trackingNumber}")]
        public async Task<ActionResult<SubscriptionStatusResponseDto>> GetSubscriptionStatus(
            [FromRoute] string postalCode,
            [FromRoute] string trackingNumber,
            CancellationToken ct)
        {
            var sub =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No 'sub' (or nameidentifier) claim in token");
            }

            var customer = await _customerRepo.GetByIdentityProviderUserIdAsync(sub, ct);
            if (customer is null)
            {
                return Forbid();
            }

            var shipment = await _shipmentRepo.GetShipmentByTrackingNumberAndPostalCode(postalCode, trackingNumber, ct);
            if(shipment is null)
            {
                return NotFound(new { message = "No shipment found for given tracking number or postal code" });
            }
         

            var subscribed = await _notificationSubscriptionRepo.ExistAsync(shipment.shipmentId, customer.customerId, ct);
            return Ok(new SubscriptionStatusResponseDto { Subscribed = subscribed });
        }



        // POST api/notifications/subscribe
        [Authorize]
        [HttpPost("subscribe")]
        public async Task<ActionResult<SubscriptionStatusResponseDto>> Sub(
            [FromBody] TrackingStatusRequestDto dto,
             CancellationToken ct)
        {
            var trackingNumber = dto?.TrackingNumber ?? "".Trim();
            var postalCode = dto?.PostalCode ?? "".Trim();

            if (string.IsNullOrWhiteSpace(postalCode) || string.IsNullOrWhiteSpace(trackingNumber))
            {
                return BadRequest(new { message = "TrackingNumber and Postal code are required." });
            }

            var sub =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No 'sub' (or nameidentifier) claim in token");
            }

            var customer = await _customerRepo.GetByIdentityProviderUserIdAsync(sub, ct);
            if(customer is null)
            {
                return Forbid();
            }

            var shipment = await _shipmentRepo.GetShipmentByTrackingNumberAndPostalCode(postalCode, trackingNumber, ct);
            if(shipment is null)
            {
                return NotFound(new { message = "No shipment found for given tracking number + postal code." });
            }

            await _notificationSubscriptionRepo.SubscribeAsync(shipment.shipmentId, customer.customerId, ct);
            return Ok(new SubscriptionStatusResponseDto { Subscribed = true });
        }


        [Authorize]
        [HttpPost("unsubscribe")]
        public async Task<ActionResult<SubscriptionStatusResponseDto>> unSub(
            [FromBody] TrackingStatusRequestDto dto,
            CancellationToken ct)
        {
            var trackingNumber = dto.TrackingNumber ?? "".Trim();
            var postalCode = dto.PostalCode ?? "".Trim();

            if (string.IsNullOrWhiteSpace(trackingNumber) || string.IsNullOrWhiteSpace(postalCode))
            {
                return BadRequest(new { message = "TrackingNumber and PostalCode are required." });
            }

            var sub =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No 'sub' (or nameidentifier) claim in token");
            }

            var customer = await _customerRepo.GetByIdentityProviderUserIdAsync(sub, ct);
            if(customer is null)
            {
                return Forbid();
            }

            var shipment = await _shipmentRepo.GetShipmentByTrackingNumberAndPostalCode(postalCode, trackingNumber, ct);
            if (shipment is null)
            {
                return NotFound(new { message = "No shipment found for given tracking number + postal code." });
            }

            await _notificationSubscriptionRepo.UnSubscribeAsync(shipment.shipmentId, customer.customerId, ct);
            return Ok(new SubscriptionStatusResponseDto { Subscribed = false });

        }
    }
}
