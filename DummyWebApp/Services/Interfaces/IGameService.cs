using DummyWebApp.ResponseModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface IGameService
    {
        public Task<GameResponseWithCompany?> GetGameById(int id);
        public Task<IEnumerable<GameResponseWithCompany>> GetAllGames();
        public Task<GameResponseWithCompany?> UpdateGame(int id, PostgreSQL.DataModels.Game game);
        public Task<bool> DeleteGame(int id);
        public Task<IEnumerable<GameResponseWithCompany>> AddGame(PostgreSQL.DataModels.Game game);

    }
}
