using DummyWebApp.Models;
namespace DummyWebApp.Services.Interfaces.Game
{
    public interface IGameService
    {
        public Task<Models.Game?> GetGameById(int id);
        public Task<IEnumerable<Models.Game>> GetAllGames();
        public Task<bool> UpdateGame(int id, Models.Game game);
        public Task<bool> DeleteGame(int id);
        public Task<IEnumerable<Models.Game>> AddGame(Models.Game game);

    }
}
