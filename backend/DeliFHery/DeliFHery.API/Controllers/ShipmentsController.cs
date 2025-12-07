using DeliFHery.API.Dto;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using DeliFHery.API.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliFHery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly ICustomerRepo _customerRepo;
        private readonly IShipmentRepo _shipmentRepo;

        public ShipmentsController(IShipmentService shipmentServie, 
                                   ICustomerRepo customerRepo,
                                   IShipmentRepo shipmentRepo)
        {
            _shipmentService = shipmentServie;
            _customerRepo = customerRepo;
            _shipmentRepo = shipmentRepo;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetallShipments([FromRoute] Guid customerId, CancellationToken ct)
        {
            var result = await _shipmentRepo.GetShipmentsForCustomer(customerId, ct);
            if (result == null)
            {
             return NotFound();
            }
            return Ok(result);
        }


        [Authorize]
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

            var customer = await _customerRepo.GetByIdentityProviderUserIdAsync(sub);
            if (customer == null)
                return Unauthorized("No customer found for current user");


            var result = await _shipmentService.CreateShipmentAsync(request, customer.customerId, ct);

            return Ok(result);
        }
    }
}
