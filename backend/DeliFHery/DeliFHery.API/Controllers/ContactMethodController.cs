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
    
    [ApiController]
    [Route("api/customers/currentUser/[controller]")]
    public class ContactMethodController(IContactMethodRepo _repo, ICustomerRepo _customer_repo) : ControllerBase
    {
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetALl([FromRoute] int customerId, CancellationToken ct)
        {
            var contactMethod = await _repo.ListForCustomerAsync(customerId);
            if (!contactMethod.Any())
            {
                return NotFound("Customer has no ContactMethod");
            }
            return Ok(contactMethod);
        }
        // GET /api/customers/currentUser/contactMethod/
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllContactForCustomer(CancellationToken ct)
        {
            var sub =
                 User.FindFirst("sub")?.Value ??
                 User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No sub in token");
            }
            var customer = await _customer_repo.GetByIdentityProviderUserIdAsync(sub);
            if(customer == null)
            {
                return NotFound("Customer not found for current user");
            }
            var contactMethods = await _repo.ListForCustomerAsync(customer.customerId, ct);

            if (!contactMethods.Any())
            {
                return NotFound("Not ContactMethod for current user");
            }
            return Ok(contactMethods);
        }

        // POST /api/customers/currentUser/contactMethod
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ContactMethod>> CreateContactMethodForCurrentUser(
            [FromBody] CreateContactMethodRequest request,
            CancellationToken ct)
        {
            var sub =
                User.FindFirst("sub")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized("No 'sub' claim in token");

            var customer = await _customer_repo.GetByIdentityProviderUserIdAsync(sub);
            if (customer == null)
            {
                return NotFound("Customer not found for current user");
            }

            var contact = new ContactMethod
            {
                customerId = customer.customerId,
                type = request.Type,
                value = request.Value,
                isVerified = false
            };

            var newId = await _repo.CreateAsync(contact, ct);
            contact.contactId = newId;

            return Ok(contact);
        }

    }
}
