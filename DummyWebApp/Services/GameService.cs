using AutoMapper;
using DummyWebApp.RequestModels.Game;
using DummyWebApp.ResponseModels.Game;
using DummyWebApp.Services.Interfaces;
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
        public async Task<GameResponseWithCompany?> GetGameById(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            return _mapper.Map<GameResponseWithCompany>(game);
        }

        public async Task<IEnumerable<GameResponseWithCompany>> GetAllGames()
        {
            var allGames = await _gameRepository.GetAllAsync();
            var gameResponses = _mapper.Map<IEnumerable<GameResponseWithCompany>>(allGames);
            foreach (var gameResponse in gameResponses)
            {
                var orderIds = GetOrdersForProvidedGame(gameResponse.Id);
                gameResponse.OrderIds = orderIds.ToList();
            }

            return gameResponses;
        }

        public async Task<GameResponseWithCompany?> UpdateGame(int id, UpdateGameRequest request)
        {
            var mappedRequest = _mapper.Map<Game>(request);
            var gameToUpdate = await _gameRepository.GetByIdAsync(id);
            if (gameToUpdate == null)
                return null;

            gameToUpdate.Name = mappedRequest.Name;
            gameToUpdate.Price = mappedRequest.Price;

            await _gameRepository.UpdateAsync(gameToUpdate);

            return _mapper.Map<GameResponseWithCompany?>(gameToUpdate);
        }

        public async Task<bool> DeleteGame(int id)
        {
            var isDeleted = await _gameRepository.DeleteAsync(id);
            return isDeleted;
        }

        public async Task<IEnumerable<GameResponseWithCompany>> AddGame(NewGameRequest newGame)
        {
            var mappedNewGame = _mapper.Map<Game>(newGame);
            var newList = await _gameRepository.AddAsync(mappedNewGame);
            return _mapper.Map<IEnumerable<GameResponseWithCompany>>(newList);

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
