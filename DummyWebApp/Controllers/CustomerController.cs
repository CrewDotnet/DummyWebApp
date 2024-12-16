using AutoMapper;
using DummyWebApp.Models.ErrorModel;
using DummyWebApp.Models.RequestModels.Customer;
using DummyWebApp.Models.ResponseModels.Customer;
using DummyWebApp.Presenters.Customer;
using DummyWebApp.Presenters.Erorr;
using DummyWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        // GET: api/CustomerController
        [HttpGet("~/api/Customers")]
        [ProducesResponseType(typeof(CustomersDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<List<CustomersDTO>>> Get()
        {
            var result = await _customerService.GetAllAsync();
            
            if (result.IsSuccess)
            {
                var customers = _mapper.Map<CustomersDTO>(result.Value);
                var mappedResult = new CustomersDTO { Customers = customers.Customers };
                return CustomersPresenter.PresentCustomers(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }
        
        // GET api/CustomerController/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<CustomerResponse>> Get(int id)
        {
            var result = await _customerService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<CustomerDTO>(result.Value);
                return CustomerPresenter.PresentCustomer(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // POST api/CustomerController
        [HttpPost]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<CustomerDTO>> Post([FromBody] NewCustomerRequest request)
        {
            {
                var result = await _customerService.AddAsync(request);

                if (result.IsSuccess)
                {
                    var mappedResult = _mapper.Map<CustomerDTO>(result.Value);
                    return CustomerPresenter.PresentCustomer(mappedResult);
                }

                return ErrorPresenter.PresentErrorResponse(result.Errors);
            }
        }

        // PUT api/CustomerController/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<CustomerDTO?>> Put(int id, [FromBody] UpdateCustomerRequest request)
        {
            {
                var result = await _customerService.UpdateAsync(id, request);
                if (result.IsSuccess)
                {
                    var mappedResult = _mapper.Map<CustomerDTO>(result.Value);
                    return CustomerPresenter.PresentCustomer(mappedResult);
                }

                return ErrorPresenter.PresentErrorResponse(result.Errors);
            }
        }

        // DELETE api/CustomerController/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _customerService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return true;
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }
    }
}
