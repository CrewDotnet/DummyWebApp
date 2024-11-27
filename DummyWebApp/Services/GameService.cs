using AutoMapper;
using DummyWebApp.ResponseModels;
using DummyWebApp.Services.Interfaces;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repository;
        private readonly IMapper _mapper;

        public GameService(IGameRepository repository, IMapper mapper)

        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<GameResponseWithCompany?> GetGameById(int id)
        {
            var game = await _repository.GetByIdAsync(id);
            return _mapper.Map<GameResponseWithCompany>(game);
        }

        public async Task<IEnumerable<GameResponseWithCompany>> GetAllGames()
        {
            var allGames = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<GameResponseWithCompany>>(allGames);
        }

        public async Task<GameResponseWithCompany?> UpdateGame(int id, Game update)
        {
            var game = await _repository.GetByIdAsync(id);
            if (game == null)
                return null;

            game.Name = update.Name;
            game.Price = update.Price;
            game.Company = update.Company;

            return _mapper.Map<GameResponseWithCompany?>(game);
        }

        public async Task<bool> DeleteGame(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);
            return isDeleted;
        }

        public async Task<IEnumerable<GameResponseWithCompany>> AddGame(Game newGame)
        {
            var newList = await _repository.AddAsync(newGame);
            return _mapper.Map<IEnumerable<GameResponseWithCompany>>(newList);

        }
    }
}
