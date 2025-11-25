using DeliFHery.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DeliFHery.API.Models;


namespace DeliFHery.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;
        public CustomerController(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }
        // POST /api/customers
        [HttpPost]
        public async Task<IActionResult> Create(Customer c, CancellationToken ct)
        {
            int id = await _customerRepo.CreateAsync(c, ct);
            return Ok(new { id });
        }

        // GET /api/customers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var customer = await _customerRepo.GetByIdAsync(id, ct);
            return customer is null ? NotFound() : Ok(customer);
        }
    }
}
