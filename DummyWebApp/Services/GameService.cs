using AutoMapper;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using GamesClient;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IGameApiClient _gameApiClient;

        public GameService(IGameRepository gameRepository, IMapper mapper, IOrderRepository orderRepository, IGameApiClient gameApiClient)

        {
            _gameRepository = gameRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _gameApiClient = gameApiClient;
        }
        public async Task<Result<GameDTO>> GetGameById(int id)
        {
            var getGame = await _gameRepository.GetByIdAsync(id);
            if (getGame.IsFailed)
            {
                return getGame.ToResult();
            }
            var result = _mapper.Map<GameDTO>(getGame.Value);
            return Result.Ok(result).WithSuccess("200 Ok");
        }

        public async Task<Result<List<GameDTO>>> GetAllGames()
        {
            var getAllGames = await _gameRepository.GetAllAsync();
            if (getAllGames.IsFailed)
            {
                return getAllGames.ToResult();
            }
            var mappedGamesList = _mapper.Map<List<GameDTO>>(getAllGames.Value);
            foreach (var game in mappedGamesList)
            {
                var orderIds = GetOrdersForProvidedGame(game.Id);
                game.OrderIds = orderIds.Result.Value.ToList();
            }

            return Result.Ok(mappedGamesList).WithSuccess("200 Ok");
        }

        public async Task<Result<GameDTO>> UpdateGame(int id, UpdateGameRequest request)
        {
            var mappedRequest = _mapper.Map<Game>(request);
            var gameToUpdate = await _gameRepository.GetByIdAsync(id);
            if (gameToUpdate.IsFailed)
            {
                return gameToUpdate.ToResult();
            }

            gameToUpdate.Value.Title = mappedRequest.Title;
            gameToUpdate.Value.Price = mappedRequest.Price;

            await _gameRepository.UpdateAsync(gameToUpdate.Value);

            var result =  _mapper.Map<GameDTO>(gameToUpdate.Value);
            return Result.Ok(result).WithSuccess("200 Ok"); ;
        }

        public async Task<Result<bool>> DeleteGame(int id)
        {
            var result = await _gameRepository.DeleteAsync(id);
            if (result.IsFailed)
            {
                return result.ToResult();
            }
            return Result.Ok(result.Value).WithSuccess("200 Ok");
        }

        public async Task<Result<GameDTO>> AddGame(NewGameRequest request)
        {

            var mappedRequest = _mapper.Map<Game>(request);
            var result = await _gameRepository.AddAsync(mappedRequest);

            if (result.IsFailed)
            {
                return result.ToResult();
            }
            var response = _mapper.Map<GameDTO>(mappedRequest);

            return Result.Ok(response).WithSuccess("200 Ok");
        }

        private async Task<Result<IEnumerable<int>>> GetOrdersForProvidedGame(int id)
        {
            var ordersResult = await _orderRepository.GetAllAsync();
            if (ordersResult.IsFailed)
            {
                return Result.Fail<IEnumerable<int>>(ordersResult.Errors);
            }
            var orderIds = ordersResult.Value
                .Where(o => o.Games!.Any(g => g.Id == id))
                .Select(o => o.Id);
            return Result.Ok(orderIds).WithSuccess("200 Ok");
        }


    }
}
