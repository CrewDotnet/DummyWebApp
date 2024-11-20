using DummyWebApp.Data;
using DummyWebApp.Models;
using DummyWebApp.Repositories;
using DummyWebApp.Services.Interfaces;
using DummyWebApp.Services.Interfaces.Game;
using Microsoft.AspNetCore.Mvc;

namespace DummyWebApp.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repository;

        public GameService(IGameRepository repository)

        {
            _repository = repository;
        }
        public Task<Game?> GetGameById(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<IEnumerable<Game>> GetAllGames()
        {
            return _repository.GetGamesAsync();
        }

        public async Task<bool> UpdateGame(int id, Game update)
        {
            var game = await _repository.GetByIdAsync(id);
            if (game == null)
                return false;

            game.Name = update.Name;
            game.Price = update.Price;
            game.Platform = update.Platform;

            return true;
        }

        public async Task<bool> DeleteGame(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);
            return isDeleted;
        }

        public async Task<IEnumerable<Game>> AddGame(Game newGame)
        {
            var newList = await _repository.AddAsync(newGame);
            return newList;

        }
    }
}
