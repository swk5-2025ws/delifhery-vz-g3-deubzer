using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliFHery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        public TestController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendTestEmail([FromQuery] string to, CancellationToken ct)
        {
            await _emailSender.SendAsync(
                toEmail: to,
                subject: "In Verteilung",
                body: "succes",
                ct: ct);

            return Ok(new { sent = true, to });
        }
    }
}
