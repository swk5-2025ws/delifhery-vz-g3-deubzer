using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliFHery.API.Controllers
{
    [Route("api/shipping")]
    [ApiController]
    public class ShipmentPricingController : ControllerBase
    {
        private readonly IShippingPriceCalculator _shippingPriceCalculator;

        public ShipmentPricingController(IShippingPriceCalculator shippingPriceCalculator)
        {
            _shippingPriceCalculator = shippingPriceCalculator;
        }

        [HttpPost("calculate")]
        [AllowAnonymous]
        public async Task<ActionResult<CalculateShipmentPriceResponseDto>> Calculate(
            [FromBody] CalculateShipmentPriceRequestDto request)
        {
            try
            {
                var result = await _shippingPriceCalculator.CalculatePriceAsync(request);
                return Ok(result);
            }catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
