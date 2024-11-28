using DummyWebApp.RequestModels;
using DummyWebApp.RequestModels.Customer;
using DummyWebApp.ResponseModels;
using DummyWebApp.ResponseModels.Customer;
using DummyWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PostgreSQL.DataModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        // GET: api/CustomerController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> Get()
        {
            var customers = await _customerService.GetAllAsync();
            if (!customers.Any())
                return NotFound("No data found");
            return Ok(customers);
        }

        // GET api/CustomerController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> Get(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound("CustomerResponse not found");
            return Ok(customer);
        }

        // POST api/CustomerController
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> Post([FromBody] NewCustomerRequest customer)
        {
            if (customer.FirstName == string.Empty || customer.EmailAddress == string.Empty ||
                customer.LastName == string.Empty)
                return BadRequest("Bad request");
            var customers = await _customerService.AddAsync(customer);
            return Ok(customers);

        }

        // PUT api/CustomerController/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerResponse?>> Put(int id, [FromBody] UpdateCustomerRequest request)
        {
            var customerToUpdate = await _customerService.GetByIdAsync(id);
            if (request.FirstName == string.Empty || request.EmailAddress == string.Empty ||
                request.LastName == string.Empty)
                return BadRequest("Bad request");
            var customer = await _customerService.UpdateAsync(id, request);
            if (customerToUpdate.Equals(request))
                return BadRequest("Bad request");
            return Ok(customer);

        }

        // DELETE api/CustomerController/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var customers = await _customerService.GetAllAsync();
            if (customers.Any(c => c.Id == id))
            {
                var isDeleted = await _customerService.DeleteAsync(id);
                return Ok(isDeleted);
            }

            return BadRequest("Provide valid Id");
        }
    }
}
