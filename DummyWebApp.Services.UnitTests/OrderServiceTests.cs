using AutoFixture;
using AutoMapper;
using DummyWebApp.Models.ResponseModels.Order;
using FluentAssertions;
using FluentResults;
using Moq;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DummyWebApp.Models.ResponseModels.Company;

namespace DummyWebApp.Services.UnitTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IGameRepository> _mockGameRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Fixture _fixture;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockGameRepository = new Mock<IGameRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _orderService = new OrderService(
                _mockOrderRepository.Object,
                _mockGameRepository.Object,
                _mockCustomerRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsOrderDTOs_WhenSuccessful()
        {
            // Arrange
            var orders = _fixture.Create<List<Order>>();
            var orderDtos = _fixture.Create<List<OrderResponse>>();

            _mockOrderRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Ok(orders));

            _mockMapper
                .Setup(mapper => mapper.Map<List<OrderResponse>>(orders))
                .Returns(orderDtos);

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(orderDtos);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsFailure_WhenRepositoryFails()
        {
            // Arrange
            _mockOrderRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Fail(new Error("Failed to retrieve orders").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _orderService.GetAllOrdersAsync();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to retrieve orders" && e.Metadata["StatusCode"].ToString() == "500");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOrderDTO_WhenSuccessful()
        {
            // Arrange
            var orderId = _fixture.Create<int>();
            var order = _fixture.Create<Order>();
            var orderDto = _fixture.Create<OrderResponse>();

            _mockOrderRepository
                .Setup(repo => repo.GetByIdAsync(orderId))
                .ReturnsAsync(Result.Ok(order));

            _mockMapper
                .Setup(mapper => mapper.Map<OrderResponse>(order))
                .Returns(orderDto);

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(orderDto);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFailure_WhenOrderNotFound()
        {
            // Arrange
            var orderId = _fixture.Create<int>();

            _mockOrderRepository
                .Setup(repo => repo.GetByIdAsync(orderId))
                .ReturnsAsync(Result.Fail(new Error("Order not found").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Order not found" && e.Metadata["StatusCode"].ToString() == "404");
        }

        [Fact]
        public async Task PlaceOrderAsync_ReturnsOrderDTO_WhenSuccessful()
        {
            // Arrange
            var customerId = _fixture.Create<int>();
            var gameIds = _fixture.Create<List<int>>();
            var customer = _fixture.Create<Customer>();
            var games = _fixture.Create<List<Game>>();
            var order = _fixture.Create<Order>();
            var orderDto = _fixture.Create<OrderResponse>();
            var company = _fixture.Create<Company>();

            _mockOrderRepository
                .Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(Result.Ok(new Order
                {
                    Id = 16,
                    Customer = customer
                }));

            _mockCustomerRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Result.Ok(new Customer
                {
                    Id = 1,
                    EmailAddress = "www@ee.coi",
                    FirstName = "Fify",
                    LastName = "Frrr"
                }));

            _mockGameRepository
                .Setup(repo => repo.GetGameCollectionByIds(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(Result.Ok(new List<Game>
                {
                    new Game
                    {
                        Id = 73, Name = "Name1", Price = 215M,
                        Company = company
                    },
                    new Game { Id = 167, Name = "Name2", Price = 134M, Company = company },
                    new Game
                    {
                        Id = 97, Name = "Name3", Price = 41M,
                        Company = company
                    }
                }));

            _mockCustomerRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(Result.Ok(true));

            _mockMapper
                .Setup(mapper => mapper.Map<OrderResponse>(It.IsAny<Order>()))
                .Returns(orderDto);

            // Act
            var result = await _orderService.PlaceOrderAsync(customerId, gameIds);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(orderDto);
        }

        [Fact]
        public async Task PlaceOrderAsync_ReturnsFailure_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = _fixture.Create<int>();
            var gameIds = _fixture.Create<List<int>>();

            _mockCustomerRepository
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(Result.Fail(new Error("Customer not found").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _orderService.PlaceOrderAsync(customerId, gameIds);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Customer not found" && e.Metadata["StatusCode"].ToString() == "404");
        }

        [Fact]
        public async Task PlaceOrderAsync_ReturnsFailure_WhenGameRetrievalFails()
        {
            // Arrange
            var customerId = _fixture.Create<int>();
            var gameIds = _fixture.Create<List<int>>();
            var customer = _fixture.Create<Customer>();

            _mockCustomerRepository
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(Result.Ok(customer));

            _mockGameRepository
                .Setup(repo => repo.GetGameCollectionByIds(gameIds))
                .ReturnsAsync(Result.Fail(new Error("Failed to retrieve games").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _orderService.PlaceOrderAsync(customerId, gameIds);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to retrieve games" && e.Metadata["StatusCode"].ToString() == "500");
        }

        [Fact]
        public async Task GetOrdersByCustomerIdAsync_ReturnsOrderDTOs_WhenSuccessful()
        {
            // Arrange
            var customerId = _fixture.Create<int>();
            var orders = _fixture.Create<List<Order>>();
            var orderDtos = _fixture.Create<List<OrderResponse>>();

            _mockOrderRepository
                .Setup(repo => repo.GetOrdersByCustomerIdAsync(customerId))
                .ReturnsAsync(Result.Ok(orders));

            _mockMapper
                .Setup(mapper => mapper.Map<List<OrderResponse>>(orders))
                .Returns(orderDtos);

            // Act
            var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(orderDtos);
        }

        [Fact]
        public async Task GetOrdersByCustomerIdAsync_ReturnsFailure_WhenRepositoryFails()
        {
            // Arrange
            var customerId = _fixture.Create<int>();

            _mockOrderRepository
                .Setup(repo => repo.GetOrdersByCustomerIdAsync(customerId))
                .ReturnsAsync(Result.Fail(new Error("Failed to retrieve orders").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _orderService.GetOrdersByCustomerIdAsync(customerId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to retrieve orders" && e.Metadata["StatusCode"].ToString() == "500");
        }
    }
}