using AutoMapper;
using DummyWebApp.ResponseModels;
using DummyWebApp.ResponseModels.Order;
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
        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<OrderResponse> PlaceOrderAsync(int customerId, IEnumerable<int> gameIds)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new ArgumentException("Customer not found", nameof(customerId));

            var games = await _gameRepository.GetGameCollectionByIds(gameIds);
            if (!games.Any()) throw new ArgumentException("No games found for the provided IDs", nameof(gameIds));

            ApplyDiscount(games, customer);

            var order = new Order
            {
                Customer = customer,
                Games = games.ToList()
            };

            AddOrderedGamesToCustomer(games, customer);
            
            customer.TotalAmountSpent += games.Sum(g => g.Price);
            customer.LoyaltyPoints = (int)(customer.TotalAmountSpent / 20);

            await _customerRepository.UpdateAsync(customer);
            await _orderRepository.CreateOrderAsync(order);
            return _mapper.Map<OrderResponse>(order);
        }

        private static void ApplyDiscount(IEnumerable<Game> games, Customer customer)
        {
            foreach (var game in games)
                if (customer.LoyaltyPoints > 5)
                {
                    game.Price = game.Price * 0.8m;
                }
        }

        private static void AddOrderedGamesToCustomer(IEnumerable<Game> games, Customer customer)
        {
            foreach (var game in games)
            {
                if (customer.Games == null)
                    customer.Games = new List<Game>();

                if (customer.Games.All(g => g.Id != game.Id))
                {
                    customer.Games.Add(game);
                }
            }
        }

        public async Task<IEnumerable<OrderResponse>> GetByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

    }
}
