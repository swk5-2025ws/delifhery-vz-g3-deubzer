using DeliFHery.API.Dto;
using DeliFHery.API.DtoMappers;
using DeliFHery.API.Interfaces;
using DeliFHery.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace DeliFHery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController(ICustomerRepo _customerRepo) : ControllerBase
    {
        

        // GET /api/customers
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct = default)
        {
            var customers = await _customerRepo.GetAllCustomersAsync(ct);
            if (customers == null)
            {
                return NotFound();
            }
            var dtos = customers.Select(CustomerDtoMapper.ToDto);
            return Ok(dtos);
        }

        // GET /api/customers/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
        {
            var sub = User.FindFirst("sub")?.Value ??
                      User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized("No 'sub' in token");

            var customer = await _customerRepo.GetByIdAsync(id, ct);
            if (customer == null)
                return NotFound();

            var userCustomer = await _customerRepo.GetByIdentityProviderUserIdAsync(sub, ct);
            if (userCustomer == null || userCustomer.customerId != id)
                return Forbid();

            return Ok(CustomerDtoMapper.ToDto(customer));
        }


        // Post /api/customers
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> EnsureCurrentCustomer(CancellationToken ct)
        {
            var sub =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No 'sub' (or nameidentifier) claim in token");
            }


            var username =
                User.FindFirst("preferred_username")?.Value ??
                User.Identity?.Name ??
                sub;

           
            Customer? existing = null;

            try
            {
                existing = await _customerRepo.GetByIdentityProviderUserIdAsync(sub);
            }
            catch
            {

            }

            if (existing != null)
                return Ok(existing);

            var newCustomer = new Customer
            {
                identityProviderUserId = sub,
                username = username,
                created_at = DateTime.UtcNow
            };

            var newid = await _customerRepo.CreateAsync(newCustomer, ct);
            newCustomer.customerId = newid;
            var dto = CustomerDtoMapper.ToDto(newCustomer);
            return Ok(dto);

        }
    }
}
