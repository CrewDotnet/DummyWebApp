using FluentResults;
using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface IGameRepository
    {
        public Task<Result<List<Game>>> GetAllAsync();
        public Task<Result<Game>> GetByIdAsync(int id);
        public Task<Result<Game>> AddAsync(Game game);
        public Task<Result> UpdateAsync(Game request);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<List<Game>>> GetGameCollectionByIds(IEnumerable<int> ids);
    }
}
