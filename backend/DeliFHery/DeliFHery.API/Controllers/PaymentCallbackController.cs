using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliFHery.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentCallbackController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentCallbackController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("callback")]
        [AllowAnonymous]
        public async Task<IActionResult> Callback([FromBody] PaymentCallbackDto dto, CancellationToken ct)
        {
            await _paymentService.HandleCallbackAsync(dto, ct);
            return Ok();
        }
    }
}
