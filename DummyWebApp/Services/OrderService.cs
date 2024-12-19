using AutoMapper;
using DummyWebApp.Models.ResponseModels.Order;
using DummyWebApp.Services.Interfaces;
using FluentResults;
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
        public async Task<Result<List<OrderResponse>>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            if (orders.IsFailed)
            {
                return orders.ToResult();
            }
            var result = _mapper.Map<List<OrderResponse>>(orders.Value);
            return Result.Ok(result);
        }

        public async Task<Result<OrderResponse>> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order.IsFailed)
            {
                return order.ToResult();
            }
            var result = _mapper.Map<OrderResponse>(order.Value);
            return Result.Ok(result);
        }

        public async Task<Result<OrderResponse>> PlaceOrderAsync(int customerId, IEnumerable<int> gameIds)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer.IsFailed)
            {
                return customer.ToResult();
            }

            var games = await _gameRepository.GetGameCollectionByIds(gameIds);
            if (games.IsFailed)
            {
                return games.ToResult();
            }

            ApplyDiscount(games.Value, customer.Value);

            var order = new Order
            {
                Customer = customer.Value,
                Games = games.Value.ToList()
            };

            AddOrderedGamesToCustomer(games.Value, customer.Value);

            SummPriceOfGamesBought(customer, games);

            CalculateLoyaltyPoints(customer);

            var updateResult = await _customerRepository.UpdateAsync(customer.Value);
            if (updateResult.IsFailed)
            {
                return Result.Fail<OrderResponse>(updateResult.Errors);
            }
            var result = await _orderRepository.CreateOrderAsync(order);
            if (result.IsFailed)
            {
                return result.ToResult();
            }
            
            var mappedResult = _mapper.Map<OrderResponse>(order);
            return Result.Ok(mappedResult);
        }

        private static void CalculateLoyaltyPoints(Result<Customer> customer)
        {
            customer.Value.LoyaltyPoints = (int)(customer.Value.TotalAmountSpent / 20);
        }

        private static void SummPriceOfGamesBought(Result<Customer> customer, Result<List<Game>> games)
        {
            customer.Value.TotalAmountSpent += games.Value.Sum(g => g.Price);
        }

        private static void ApplyDiscount(IEnumerable<Game> games, Customer customer)
        {
            foreach (var game in games)
                if (customer.LoyaltyPoints > 5)
                {
                    game.Price *= 0.8m;
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

        public async Task<Result<List<OrderResponse>>> GetOrdersByCustomerIdAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            if (orders.IsFailed)
            {
                return orders.ToResult();
            }
            if (!orders.Value.Any())
            {
                return orders.ToResult();
            }

            var result = _mapper.Map<List<OrderResponse>>(orders.Value);
            return Result.Ok(result);
        }

    }
}
