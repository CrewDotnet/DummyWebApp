using Microsoft.EntityFrameworkCore;
using PostgreSQL.Data;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace PostgreSQL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApiContext _context;
        public GameRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            var list = await _context.Games
                .Include(g => g.Company)
                .ToListAsync();
            return list;
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Game>> AddAsync(Game newGame)
        {
            if (_context.Games.Any(g => g.Name == newGame.Name))
                throw new ArgumentException("Game with same name already exists");

            newGame.Id = _context.Games.Max(g => g.Id) + 1;
            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();
            return _context.Games.ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gameToDelete = await _context.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (gameToDelete == null)
                return false;
            _context.Games.Remove(gameToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
