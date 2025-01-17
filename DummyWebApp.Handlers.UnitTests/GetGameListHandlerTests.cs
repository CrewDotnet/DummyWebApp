using DummyWebApp.MediatR.Handlers;
using DummyWebApp.MediatR.Queries;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentAssertions;
using FluentResults;
using Moq;
using Xunit;

namespace DummyWebApp.Handlers.UnitTests
{
    public class GetGameListHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly GetGameListHandler _handler;

        public GetGameListHandlerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _handler = new GetGameListHandler(_gameServiceMock.Object);
        }

        [Fact]
        public async Task GetGameListHandler_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetGameListQuery();
            var serviceResult = Result.Ok(new List<GameDTO>
            {
                new()
                {
                    Id = 1,
                    Title = "Game Title",
                    Price = 29.99m,
                }
            });
            _gameServiceMock.Setup(service => service.GetAllGames())
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Count.Should().Be(1);
            result.Value.First().Id.Should().Be(1);
            result.Value.First().Title.Should().Be("Game Title");
            result.Value.First().Price.Should().Be(29.99m);

            // Verify that the service method was called once
            _gameServiceMock.Verify(service => service.GetAllGames(), Times.Once);
        }

        [Fact]
        public async Task GetGameListHandler_EmptyList_ReturnsSuccessResultWithEmptyList()
        {
            // Arrange
            var query = new GetGameListQuery();
            var serviceResult = Result.Ok(new List<GameDTO>());
            _gameServiceMock.Setup(service => service.GetAllGames())
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEmpty();

            // Verify that the service method was called once
            _gameServiceMock.Verify(service => service.GetAllGames(), Times.Once);
        }

        [Fact]
        public async Task GetGameListHandler_ServiceReturnsError_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetGameListQuery();
            var serviceResult = Result.Fail<List<GameDTO>>("Service error");
            _gameServiceMock.Setup(service => service.GetAllGames())
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Message == "Service error");

            // Verify that the service method was called once
            _gameServiceMock.Verify(service => service.GetAllGames(), Times.Once);
        }
    }
}