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
            return Ok(customers);
        }

        // GET /api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var customer = await _customerRepo.GetByIdAsync(id, ct);
            if(customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // Post /api/customers/currentUser
        [Authorize]
        [HttpPost("currentUser")]
        public async Task<ActionResult<Customer>> EnsureCurrentCustomer(CancellationToken ct)
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

            var newId = await _customerRepo.CreateAsync(newCustomer, ct);
            newCustomer.customerId = newId;

            return Ok(newCustomer);
        }
    }
}
