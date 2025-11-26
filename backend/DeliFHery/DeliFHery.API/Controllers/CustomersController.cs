using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DeliFHery.API.Models;


namespace DeliFHery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController( ICustomerRepo _customerRepo) : ControllerBase
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

        // POST /api/customers
        [HttpPost]
        public async Task<IActionResult> Create(Customer c, CancellationToken ct)
        {
            int id = await _customerRepo.CreateAsync(c, ct);
            return Ok(new { id });
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
    }
}
