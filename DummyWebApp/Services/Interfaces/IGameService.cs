using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Models.ResponseModels.Game;
using FluentResults;
using PostgreSQL.DataModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface IGameService
    {
        public Task<Result<GameDTO>> GetGameById(int id);
        public Task<Result<List<GameDTO>>> GetAllGames();
        public Task<Result<GameDTO>> UpdateGame(int id, UpdateGameRequest request);
        public Task<Result<bool>> DeleteGame(int id);
        public Task<Result<GameDTO>> AddGame(NewGameRequest game);

    }
}
