using AutoMapper;
using DummyWebApp.ResponseModels;
using DummyWebApp.ResponseModels.Order;
using DummyWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PostgreSQL.DataModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: api/OrderController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET api/OrderController/Order ID
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponse>> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            return Ok(order);
        }

        // GET api/OrderController/Customer ID
        [HttpGet("CustomerOrder/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderService.GetByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // POST api/OrderController
        [HttpPost]
        public async Task<OrderResponse> Post([FromQuery] int customerId, [FromBody] IEnumerable<int> gameIds)
        {
            var newOrder = await _orderService.PlaceOrderAsync(customerId, gameIds);
            return newOrder;
        }
    }
}
