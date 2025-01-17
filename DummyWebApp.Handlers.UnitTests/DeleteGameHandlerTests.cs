using AutoMapper;
using DummyWebApp.MediatR.Commands;
using DummyWebApp.MediatR.Handlers;
using DummyWebApp.Services.Interfaces;
using FluentAssertions;
using FluentResults;
using Moq;

namespace DummyWebApp.Handlers.UnitTests
{
    public class DeleteGameHandlerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteGameHandler _handler;
        public DeleteGameHandlerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _handler = new DeleteGameHandler(_gameServiceMock.Object);
        }
        [Fact]
        public async Task HandleDeleteGame_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var command = new DeleteGameCommand(1);
            var serviceResult = Result.Ok(true);

            _gameServiceMock.Setup(service => service.DeleteGame(It.IsAny<int>()))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();
            _gameServiceMock.Verify(service => service.DeleteGame(
                    It.Is<int>(id => id == command.Id)),
                Times.Once);
        }
        [Fact]
        public async Task HandleDeleteGame_InvalidRequest_ReturnsFailedResult()
        {
            // Arrange
            var command = new DeleteGameCommand(1);
            var serviceResult = Result.Fail("Error");
            _gameServiceMock.Setup(service => service.DeleteGame(It.IsAny<int>()))
                .ReturnsAsync(serviceResult);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Message.Should().Be("Error");
            _gameServiceMock.Verify(service => service.DeleteGame(
                    It.Is<int>(id => id == command.Id)),
                Times.Once);
        }
    }
}
