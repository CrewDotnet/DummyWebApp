using AutoMapper;
using DummyWebApp.MediatR.Commands;
using DummyWebApp.MediatR.Handlers;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Services.Interfaces;
using Moq;
using FluentAssertions;
using FluentResults;
using DummyWebApp.Models.ResponseModels.Game;

namespace DummyWebApp.Handlers.UnitTests
{
    public class AddGameHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddGameHandler _handler;

        public AddGameHandlerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddGameHandler(_gameServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task HandleAddGame_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var command = new AddGameCommand("Game Title", 29.99m, 1);
            var serviceResult = Result.Ok(new GameDTO
            {
                Id = 1,
                Title = "Game Title",
                Price = 29.99m,
                
            });

            var mappedResponse = new GameBaseResponse
            {
                Id = 1,
                Title = "Game Title",
                Price = 29.99m,
            };

            _gameServiceMock.Setup(service => service.AddGame(It.IsAny<NewGameRequest>()))
                .ReturnsAsync(serviceResult);

            _mapperMock.Setup(mapper => mapper.Map<GameBaseResponse>(It.IsAny<GameDTO>()))
                .Returns(mappedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(mappedResponse.Id);
            result.Value.Title.Should().Be(mappedResponse.Title);
            result.Value.Price.Should().Be(mappedResponse.Price);

            _gameServiceMock.Verify(service => service.AddGame(
                    It.Is<NewGameRequest>(r => r.Title == command.Title && r.Price == command.Price && r.CompanyId == command.CompanyId)),
                Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<GameBaseResponse>(serviceResult.Value), Times.Once);
        }

        [Fact]
        public async Task HandleAddGame_FailedServiceCall_ReturnsFailedResult()
        {
            // Arrange
            var command = new AddGameCommand("Game Title", 29.99m, 1);
            var serviceError = Result.Fail<GameDTO>(new List<Error>
        {
            new("Service failed.")
        });

            // Mocking service
            _gameServiceMock.Setup(service => service.AddGame(It.IsAny<NewGameRequest>()))
                .ReturnsAsync(serviceError);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle()
                .Which.Message.Should().Be("Service failed.");

            _gameServiceMock.Verify(service => service.AddGame(
                    It.Is<NewGameRequest>(r => r.Title == command.Title && r.Price == command.Price && r.CompanyId == command.CompanyId)),
                Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<GameBaseResponse>(It.IsAny<GameDTO>()), Times.Never);
        }
    }
}