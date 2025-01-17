using DummyWebApp.MediatR.Commands;
using DummyWebApp.MediatR.Handlers;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using Moq;
using DummyWebApp.Models.RequestModels.Game;
using FluentAssertions;

namespace DummyWebApp.Handlers.UnitTests
{
    public class UpdateGameHandlerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly UpdateGameHandler _handler;

        public UpdateGameHandlerTests()
        {
            _mockGameService = new Mock<IGameService>();
            _handler = new UpdateGameHandler(_mockGameService.Object);
        }

        [Fact]
        public async Task HandleUpdateGame_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var updateGameCommand = new UpdateGameCommand
            (
                1,
                "Action",
                "An action game",
                "PC"
            );

            var gameResponse = new GameDTO
            {
                Id = 1,
                Title = "Game Title",
                Price = 59.99m,
                Genre = "Action",
                ShortDescription = "An action game",
                Platform = "PC"
            };

            _mockGameService.Setup(service => service.UpdateGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .ReturnsAsync(Result.Ok(gameResponse));

            // Act
            var result = await _handler.Handle(updateGameCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Genre.Should().Be(gameResponse.Genre);
            result.Value.ShortDescription.Should().Be(gameResponse.ShortDescription);
            result.Value.Platform.Should().Be(gameResponse.Platform);

        }

        [Fact]
        public async Task HandleUpdateGame_InvalidRequest_ReturnsFailedResult()
        {
            // Arrange
            var updateGameCommand = new UpdateGameCommand
            (
                1,
                "Action",
                "An action game",
                "PC"
            );

            _mockGameService.Setup(service => service.UpdateGame(It.IsAny<int>(), It.IsAny<UpdateGameRequest>()))
                .ReturnsAsync(Result.Fail<GameDTO>("Update failed"));

            // Act
            var result = await _handler.Handle(updateGameCommand, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Message.Should().Be("Update failed");
        }
    }
}
