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
    [Authorize]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentServie)
        {
            _shipmentService = shipmentServie;
        }

        [HttpPost]
        public async Task<ActionResult<CreateShipmentResponseDto>> CreateShipment([FromBody] CreateShipmentRequestDto request,
                                                                                    CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sub =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No sub in token");
            }

            if (!Guid.TryParse(sub, out var customerId))
                return Unauthorized("User ID missing or invalid");


            var result = await _shipmentService.CreateShipmentAsync(request, customerId, ct);

            return Ok(result);
        }
    }
}
