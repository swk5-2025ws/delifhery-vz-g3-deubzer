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
    [Route("api/customers/me/[controller]")]
    public class ContactMethodController(IContactMethodRepo _repo, ICustomerRepo _customer_repo) : ControllerBase
    {

        // GET /api/customers/{customerId}/contactMethod

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


            return Ok(contactMethods);
        }
         
        // POST /api/customers/<customerId>/contactMethod
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ContactMethod>> CreateContactMethodForCurrentUser(
            [FromBody] CreateContactMethodRequestDto request,
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
                isPrimary = request.IsPrimary,
            };

            var newId = await _repo.CreateAsync(contact, ct);
            contact.contactId = newId;

            return Ok(contact);
        }

        [Authorize]
        [HttpDelete("{contactId:int}")]
        public async Task<ActionResult> DeleteContactMehtod([FromRoute] int contactId, CancellationToken ct)
        {
            var sub =
                 User.FindFirst("sub")?.Value ??
                 User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(sub))
            {
                return Unauthorized("No sub in token");
            }
            var customer = await _customer_repo.GetByIdentityProviderUserIdAsync(sub);
            if (customer == null)
            { 
               return NotFound("Customer not found");
            }
            var deleted = await _repo.DeleteAsync(customer.customerId, contactId, ct);

            if (!deleted)
            {
                return NotFound("Contact Method not found");
            }

            return NoContent();
        }

    }
}
