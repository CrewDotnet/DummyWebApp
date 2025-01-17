using FluentResults;

namespace GamesClient
{
    public interface IGameApiClient
    {
        Task<Result<GameClientModel>> GetClientGameByIdAsync(int id);
        Task<Result<IEnumerable<GameClientModel>?>> GetAllClientGames();
    }
}
