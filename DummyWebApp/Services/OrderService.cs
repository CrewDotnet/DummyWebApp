using AutoMapper;
using DummyWebApp.ResponseModels;
using DummyWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IGameRepository gameRepository, ICustomerRepository customerRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _gameRepository = gameRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;

        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders;
        }

        public async Task<OrderResponse> PlaceOrderAsync(int customerId, IEnumerable<int> gameIds)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new ArgumentException("Customer not found", nameof(customerId));

            var games = await _gameRepository.GetGameCollectionByIds(gameIds);
            if (!games.Any()) throw new ArgumentException("No games found for the provided IDs", nameof(gameIds));
            var order = new Order
            {
                Customer = customer,
                Games = games.ToList()
            };
            await _orderRepository.CreateOrderAsync(order);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return orders;
        }
    }
}
