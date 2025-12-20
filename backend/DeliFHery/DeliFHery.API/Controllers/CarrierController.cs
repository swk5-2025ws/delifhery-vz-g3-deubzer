using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliFHery.API.Controllers
{
    [Route("api/carrier/tracking")]
    [ApiController]
    public class CarrierController : ControllerBase
    {
        private readonly ICarrierTrackingService _carrierTrackingService;
        private readonly ICustomerRepo _customerRepo;
        private readonly ICarrierRepo _carrierRepo;
        public CarrierController(ICarrierTrackingService carrierTrackingService,
                                 ICustomerRepo customerRepo,
                                 ICarrierRepo carrierRepo)
        {
            _carrierTrackingService = carrierTrackingService;
            _customerRepo = customerRepo;
            _carrierRepo = carrierRepo;
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

        [HttpPost("new")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CarrierCreateDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.name))
            {
                return BadRequest(new { message = "name is required" });
            }

            var carrier = new Carrier
            {
                name = dto.name,
                apiKey = dto.apiKey,
                isActive = dto.isActive,
            };

            var id = await _carrierRepo.CreateAsync(carrier, ct);

            return NoContent();
                
        }
    }
}
