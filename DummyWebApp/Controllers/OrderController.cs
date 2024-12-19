using AutoMapper;
using DummyWebApp.Models.ResponseModels.Order;
using DummyWebApp.Presenters.Erorr;
using DummyWebApp.Presenters.Order;
using DummyWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        // GET: api/OrderController
        [HttpGet("~/api/Orders")]
        public async Task<ActionResult<List<OrderResponse>>> GetAllOrdersAsync()
        {
            var result = await _orderService.GetAllOrdersAsync();

            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<List<OrderResponse>>(result.Value);
                return OrdersPresenter.PresentOrders(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // GET api/OrderController/Order ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrderByIdAsync(int id)
        {
            var result = await _orderService.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<OrderResponse>(result.Value);
                return OrderPresenter.PresentOrder(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // GET api/OrderController/Customer ID
        [HttpGet("CustomerOrder/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByCustomerId(int customerId)
        {
            var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);

            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<List<OrderResponse>>(result.Value);
                return OrdersPresenter.PresentOrders(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // POST api/OrderController
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> Post([FromQuery] int customerId, [FromBody] IEnumerable<int> gameIds)
        {
            var result = await _orderService.PlaceOrderAsync(customerId, gameIds);

            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<OrderResponse>(result.Value);
                return OrderPresenter.PresentOrder(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }
    }
}
