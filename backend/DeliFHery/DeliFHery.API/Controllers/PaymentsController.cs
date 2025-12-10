using DeliFHery.API.Interfaces;
using DeliFHery.API.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliFHery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ICustomerRepo _customerRepo;

        public PaymentsController(IPaymentService paymentService, ICustomerRepo customerRepo)
        {
            _paymentService = paymentService;
            _customerRepo = customerRepo;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string paymentId, CancellationToken ct)
        {
            var sub =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No sub in token");
            }

            var customer = await _customerRepo.GetByIdentityProviderUserIdAsync(sub);
            if (customer == null)
                return Unauthorized("No customer found for current user");

            var result = await _paymentService.GetPaymentSummaryForCustomerAsync(paymentId,customer.customerId, ct);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
