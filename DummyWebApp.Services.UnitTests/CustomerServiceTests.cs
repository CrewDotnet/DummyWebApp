using AutoFixture;
using AutoMapper;
using DummyWebApp.Models.RequestModels.Customer;
using DummyWebApp.Models.ResponseModels.Customer;
using DummyWebApp.Services;
using FluentAssertions;
using FluentResults;
using Moq;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services.UnitTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Fixture _fixture;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _fixture = new Fixture();
            _mockRepository = new Mock<ICustomerRepository>();
            _mockMapper = new Mock<IMapper>();
            _customerService = new CustomerService(_mockRepository.Object, _mockMapper.Object);
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetAllAsync_ReturnsCustomerList_WhenSuccessful()
        {
            // Arrange
            var customers = _fixture.Create<List<Customer>>();
            var customerResponses = _fixture.Create<List<CustomerResponse>>();

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Ok(customers));

            _mockMapper
                .Setup(mapper => mapper.Map<List<CustomerResponse>>(customers))
                .Returns(customerResponses);

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(customerResponses);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsFailure_WhenRepositoryFails()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Fail(new Error("No customers found").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _customerService.GetAllAsync();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "No customers found" && e.Metadata["StatusCode"].ToString() == "404");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCustomer_WhenSuccessful()
        {
            // Arrange
            var customerId = _fixture.Create<int>();
            var customer = _fixture.Create<Customer>();
            var customerResponse = _fixture.Create<CustomerResponse>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(Result.Ok(customer));

            _mockMapper
                .Setup(mapper => mapper.Map<CustomerResponse>(customer))
                .Returns(customerResponse);

            // Act
            var result = await _customerService.GetByIdAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(customerResponse);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFailure_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = _fixture.Create<int>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(Result.Fail(new Error("Customer not found").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _customerService.GetByIdAsync(customerId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Customer not found" && e.Metadata["StatusCode"].ToString() == "404");
        }

        [Fact]
        public async Task AddAsync_ReturnsCustomerResponse_WhenSuccessful()
        {
            // Arrange
            var request = _fixture.Create<NewCustomerRequest>();
            var customerEntity = _fixture.Create<Customer>();
            var customerResponse = _fixture.Create<CustomerResponse>();

            _mockMapper.Setup(mapper => mapper.Map<Customer>(request)).Returns(customerEntity);
            _mockMapper.Setup(mapper => mapper.Map<CustomerResponse>(customerEntity)).Returns(customerResponse);

            _mockRepository
                .Setup(repo => repo.AddAsync(customerEntity))
                .ReturnsAsync(Result.Ok(customerEntity));

            // Act
            var result = await _customerService.AddAsync(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(customerResponse);
        }

        [Fact]
        public async Task AddAsync_ReturnsFailure_WhenRepositoryFails()
        {
            // Arrange
            var request = _fixture.Create<NewCustomerRequest>();
            var customerEntity = _fixture.Create<Customer>();

            _mockMapper.Setup(mapper => mapper.Map<Customer>(request)).Returns(customerEntity);

            _mockRepository
                .Setup(repo => repo.AddAsync(customerEntity))
                .ReturnsAsync(Result.Fail(new Error("Failed to add customer").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _customerService.AddAsync(request);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to add customer");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenDeletionIsSuccessful()
        {
            // Arrange
            var customerId = _fixture.Create<int>();

            _mockRepository
                .Setup(repo => repo.DeleteAsync(customerId))
                .ReturnsAsync(Result.Ok(true));

            // Act
            var result = await _customerService.DeleteAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFailure_WhenDeletionFails()
        {
            // Arrange
            var customerId = _fixture.Create<int>();

            _mockRepository
                .Setup(repo => repo.DeleteAsync(customerId))
                .ReturnsAsync(Result.Fail(new Error("Failed to delete customer").WithMetadata("StatusCode", 400)));

            // Act
            var result = await _customerService.DeleteAsync(customerId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to delete customer" && e.Metadata["StatusCode"].ToString() == "400");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsUpdatedCustomerResponse_WhenSuccessful()
        {
            // Arrange
            var customerId = _fixture.Create<int>();
            var request = _fixture.Create<UpdateCustomerRequest>();
            var customer = _fixture.Create<Customer>();
            var customerResponse = _fixture.Create<CustomerResponse>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(Result.Ok(customer));

            _mockMapper
                .Setup(mapper => mapper.Map<CustomerResponse>(customer))
                .Returns(customerResponse);

            // Act
            var result = await _customerService.UpdateAsync(customerId, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            customer.FirstName.Should().Be(request.FirstName);
            _mockRepository.Verify(repo => repo.UpdateAsync(customer), Times.Once);
            result.Value.Should().BeEquivalentTo(customerResponse);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFailure_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = _fixture.Create<int>();
            var request = _fixture.Create<UpdateCustomerRequest>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(customerId))
                .ReturnsAsync(Result.Fail(new Error("Customer not found").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _customerService.UpdateAsync(customerId, request);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Customer not found" && e.Metadata["StatusCode"].ToString() == "404");
        }
    }
}


