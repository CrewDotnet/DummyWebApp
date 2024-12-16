using AutoMapper;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Company;
using DummyWebApp.Models.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GameService(IGameRepository gameRepository, IMapper mapper, IOrderRepository orderRepository)

        {
            _gameRepository = gameRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<Result<GameDTO>> GetGameById(int id)
        {
            var getGame = await _gameRepository.GetByIdAsync(id);
            if (getGame.IsFailed)
            {
                return getGame.ToResult();
            }
            var result = _mapper.Map<GameDTO>(getGame.Value);
            return Result.Ok(result);
        }

        public async Task<Result<List<GameDTO>>> GetAllGames()
        {
            var getAllGames = await _gameRepository.GetAllAsync();
            if (getAllGames.IsFailed)
            {
                return getAllGames.ToResult();
            }
            var results = _mapper.Map<List<GameDTO>>(getAllGames.Value);
            foreach (var result in results)
            {
                var orderIds = GetOrdersForProvidedGame(result.Id);
                result.OrderIds = orderIds.ToList();
            }

            return Result.Ok(results);
        }

        public async Task<Result<GameDTO>> UpdateGame(int id, UpdateGameRequest request)
        {
            var mappedRequest = _mapper.Map<Game>(request);
            var gameToUpdate = await _gameRepository.GetByIdAsync(id);
            if (gameToUpdate.Value == null)
            {
                return gameToUpdate.ToResult();
            }

            gameToUpdate.Value.Name = mappedRequest.Name;
            gameToUpdate.Value.Price = mappedRequest.Price;

            await _gameRepository.UpdateAsync(gameToUpdate.Value);

            var result =  _mapper.Map<GameDTO>(gameToUpdate.Value);
            return Result.Ok(result);
        }

        public async Task<Result<bool>> DeleteGame(int id)
        {
            var result = await _gameRepository.DeleteAsync(id);
            if (result.IsFailed)
            {
                return result.ToResult();
            }
            return Result.Ok(result.Value);
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

            return Result.Ok(response);
        }

        private IEnumerable<int> GetOrdersForProvidedGame(int id)
        {
            var orders = _orderRepository.GetAllAsync();
            var orderIds = orders.Result
                .Where(o => o.Games!.Any(g => g.Id == id))
                .Select(o => o.Id);
            return orderIds;
        }
    }
}
