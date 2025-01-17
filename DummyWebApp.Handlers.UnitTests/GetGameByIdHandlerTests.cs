using DummyWebApp.MediatR.Handlers;
using DummyWebApp.MediatR.Queries;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentAssertions;
using FluentResults;
using Moq;

namespace DummyWebApp.Handlers.UnitTests
{
    public class GetGameByIdHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly GetGameByIdHandler _handler;

        public GetGameByIdHandlerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _handler = new GetGameByIdHandler(_gameServiceMock.Object);
        }

        [Fact]
        public async Task HandleGetGameById_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetGameByIdQuery(1);
            var serviceResult = Result.Ok(new GameDTO
            {
                Id = 1,
                Title = "Game Title",
                Price = 29.99m,
            });
            _gameServiceMock.Setup(service => service.GetGameById(It.IsAny<int>()))
                .ReturnsAsync(serviceResult);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(serviceResult.Value.Id);
            result.Value.Title.Should().Be(serviceResult.Value.Title);
            result.Value.Price.Should().Be(serviceResult.Value.Price);
        }

        [Fact]
        public async Task HandleGetGameById_InvalidRequest_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetGameByIdQuery(-1);
            var serviceResult = Result.Fail<GameDTO>("Invalid game ID");
            _gameServiceMock.Setup(service => service.GetGameById(It.IsAny<int>()))
                .ReturnsAsync(serviceResult);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Message == "Invalid game ID");
        }

        [Fact]
        public async Task HandleGetGameById_ServiceThrowsException_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetGameByIdQuery(1);
            _gameServiceMock.Setup(service => service.GetGameById(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Service error"));
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Message == "Service error");
        }

        [Fact]
        public async Task HandleGetGameById_VerifyServiceMethodCall()
        {
            // Arrange
            var query = new GetGameByIdQuery(1);
            var serviceResult = Result.Ok(new GameDTO
            {
                Id = 1,
                Title = "Game Title",
                Price = 29.99m,
            });
            _gameServiceMock.Setup(service => service.GetGameById(It.IsAny<int>()))
                .ReturnsAsync(serviceResult);
            // Act
            await _handler.Handle(query, CancellationToken.None);
            // Assert
            _gameServiceMock.Verify(service => service.GetGameById(1), Times.Once);
        }
    }
}