using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetAllAsync();
        public Task<Game?> GetByIdAsync(int id);
        public Task<IEnumerable<Game>> AddAsync(Game game);
        public Task<bool> DeleteAsync(int id);
        public Task<IEnumerable<Game>> GetGameCollectionByIds(IEnumerable<int> ids);
    }
}
