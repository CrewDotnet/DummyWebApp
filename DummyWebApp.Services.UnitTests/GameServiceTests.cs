using AutoFixture;
using FluentAssertions;
using FluentResults;
using Moq;
using AutoMapper;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Game;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services.UnitTests
{
    public class GameServiceTests
    {
        private readonly Mock<IGameRepository> _mockGameRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Fixture _fixture;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _mockGameRepository = new Mock<IGameRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _gameService = new GameService(_mockGameRepository.Object, _mockMapper.Object, _mockOrderRepository.Object);
        }

        [Fact]
        public async Task GetGameById_ReturnsGameDTO_WhenSuccessful()
        {
            // Arrange
            var gameId = _fixture.Create<int>();
            var game = _fixture.Create<Game>();
            var gameDto = _fixture.Create<GameDTO>();

            _mockGameRepository
                .Setup(repo => repo.GetByIdAsync(gameId))
                .ReturnsAsync(Result.Ok(game));

            _mockMapper
                .Setup(mapper => mapper.Map<GameDTO>(game))
                .Returns(gameDto);

            // Act
            var result = await _gameService.GetGameById(gameId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(gameDto);
        }

        [Fact]
        public async Task GetGameById_ReturnsFailure_WhenGameNotFound()
        {
            // Arrange
            var gameId = _fixture.Create<int>();

            _mockGameRepository
                .Setup(repo => repo.GetByIdAsync(gameId))
                .ReturnsAsync(Result.Fail(new Error("Game not found").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _gameService.GetGameById(gameId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Game not found" && e.Metadata["StatusCode"].ToString() == "404");
        }

        [Fact]
        public async Task GetAllGames_ReturnsListOfGameDTOs_WhenSuccessful()
        {
            // Arrange
            var games = _fixture.Create<List<Game>>();
            var gameDtos = _fixture.Create<List<GameDTO>>();

            _mockGameRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Ok(games));

            _mockMapper
                .Setup(mapper => mapper.Map<List<GameDTO>>(games))
                .Returns(gameDtos);

            var orders = _fixture.Create<List<Order>>();
            _mockOrderRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(orders);

            foreach (var gameDto in gameDtos)
            {
                var orderIds = orders.Where(o => o.Games!.Any(g => g.Id == gameDto.Id)).Select(o => o.Id).ToList();
                gameDto.OrderIds = orderIds;
            }

            // Act
            var result = await _gameService.GetAllGames();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(gameDtos);
        }

        [Fact]
        public async Task GetAllGames_ReturnsFailure_WhenRepositoryFails()
        {
            // Arrange
            _mockGameRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Fail(new Error("Failed to retrieve games").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _gameService.GetAllGames();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to retrieve games" && e.Metadata["StatusCode"].ToString() == "500");
        }

        [Fact]
        public async Task AddGame_ReturnsGameDTO_WhenSuccessful()
        {
            // Arrange
            var request = _fixture.Create<NewGameRequest>();
            var game = _fixture.Create<Game>();
            var gameDto = _fixture.Create<GameDTO>();

            _mockMapper.Setup(mapper => mapper.Map<Game>(request)).Returns(game);
            _mockMapper.Setup(mapper => mapper.Map<GameDTO>(game)).Returns(gameDto);

            _mockGameRepository.Setup(repo => repo.AddAsync(game)).ReturnsAsync(Result.Ok(game));

            // Act
            var result = await _gameService.AddGame(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(gameDto);
        }

        [Fact]
        public async Task AddGame_ReturnsFailure_WhenRepositoryFails()
        {
            // Arrange
            var request = _fixture.Create<NewGameRequest>();
            var game = _fixture.Create<Game>();

            _mockMapper.Setup(mapper => mapper.Map<Game>(request)).Returns(game);

            _mockGameRepository
                .Setup(repo => repo.AddAsync(game))
                .ReturnsAsync(Result.Fail(new Error("Failed to add game").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _gameService.AddGame(request);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to add game" && e.Metadata["StatusCode"].ToString() == "500");
        }

        [Fact]
        public async Task DeleteGame_ReturnsTrue_WhenSuccessful()
        {
            // Arrange
            var gameId = _fixture.Create<int>();

            _mockGameRepository
                .Setup(repo => repo.DeleteAsync(gameId))
                .ReturnsAsync(Result.Ok(true));

            // Act
            var result = await _gameService.DeleteGame(gameId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteGame_ReturnsFailure_WhenRepositoryFails()
        {
            // Arrange
            var gameId = _fixture.Create<int>();

            _mockGameRepository
                .Setup(repo => repo.DeleteAsync(gameId))
                .ReturnsAsync(Result.Fail(new Error("Failed to delete game").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _gameService.DeleteGame(gameId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to delete game" && e.Metadata["StatusCode"].ToString() == "500");
        }

        [Fact]
        public async Task UpdateGame_ReturnsUpdatedGameDTO_WhenSuccessful()
        {
            // Arrange
            var gameId = _fixture.Create<int>();
            var request = _fixture.Build<UpdateGameRequest>()
                .With(r => r.Name, "TestName")
                .With(r => r.Price, 100)
                .Create();
            var game = _fixture.Build<Game>()
                .With(g => g.Name, "OldName")
                .With(g => g.Price, 50)
                .Create();
            var gameDto = _fixture.Build<GameDTO>()
                .With(d => d.Name, request.Name)
                .With(d => d.Price, request.Price)
                .Create();

            _mockGameRepository
                .Setup(repo => repo.GetByIdAsync(gameId))
                .ReturnsAsync(Result.Ok(game));

            _mockMapper.Setup(mapper => mapper.Map<Game>(request))
                .Returns(new Game
                {
                    Name = request.Name,
                    Price = request.Price,
                    Company = new PostgreSQL.DataModels.Company()
                    {
                        Name = "SomeCompany"
                    }
                });
            _mockMapper.Setup(mapper => mapper.Map<GameDTO>(game)).Returns(gameDto);

            // Act
            var result = await _gameService.UpdateGame(gameId, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            game.Name.Should().Be(request.Name);
            game.Price.Should().Be(request.Price);
            result.Value.Should().BeEquivalentTo(gameDto);
        }

        [Fact]
        public async Task UpdateGame_ReturnsFailure_WhenGameNotFound()
        {
            // Arrange
            var gameId = _fixture.Create<int>();
            var request = _fixture.Create<UpdateGameRequest>();

            _mockGameRepository
                .Setup(repo => repo.GetByIdAsync(gameId))
                .ReturnsAsync(Result.Fail(new Error("Game not found").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _gameService.UpdateGame(gameId, request);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Game not found" && e.Metadata["StatusCode"].ToString() == "404");
        }
    }
}

