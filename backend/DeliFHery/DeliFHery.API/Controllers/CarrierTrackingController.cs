using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliFHery.API.Controllers
{
    [Route("api/carrier/tracking")]
    [ApiController]
    public class CarrierTrackingController : ControllerBase
    {
        private readonly ICarrierTrackingService _carrierTrackingService;
        private readonly ICustomerRepo _customerRepo;
        public CarrierTrackingController(ICarrierTrackingService carrierTrackingService, ICustomerRepo customerRepo)
        {
            _carrierTrackingService = carrierTrackingService;
            _customerRepo = customerRepo;
        }

        [HttpPost("status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStatus([FromHeader(Name = "X-DeliFHery-Api-Key")] string apiKey,
            [FromBody] TrackingStatusUpdateDto dto,
            CancellationToken ct)
        {



            await _carrierTrackingService.UpdateStatusAsync(apiKey, dto, ct);
            return Ok(new { message = "Status updated" });
        }
    }
}
