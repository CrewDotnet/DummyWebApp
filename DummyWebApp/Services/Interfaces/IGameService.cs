using DummyWebApp.ResponseModels.Game;
using PostgreSQL.DataModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface IGameService
    {
        public Task<GameResponseWithCompany?> GetGameById(int id);
        public Task<IEnumerable<GameResponseWithCompany>> GetAllGames();
        public Task<GameResponseWithCompany?> UpdateGame(int id, Game game);
        public Task<bool> DeleteGame(int id);
        public Task<IEnumerable<GameResponseWithCompany>> AddGame(Game game);

    }
}
