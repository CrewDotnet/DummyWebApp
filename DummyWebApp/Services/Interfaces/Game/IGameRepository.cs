namespace DummyWebApp.Services.Interfaces.Game
{
    public interface IGameRepository
    {
        Task<IEnumerable<Models.Game>> GetGamesAsync();
        Task<Models.Game?> GetByIdAsync(int id);
        Task<IEnumerable<Models.Game>> AddAsync(Models.Game game);
        Task<bool> DeleteAsync(int id);

    }
}
