using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliFHery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string paymentId, CancellationToken ct)
        {
            var result = await _paymentService.GetPaymentSummaryAsync(paymentId, ct);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
