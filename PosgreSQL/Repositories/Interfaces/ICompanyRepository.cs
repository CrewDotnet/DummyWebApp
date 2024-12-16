using FluentResults;
using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        public Task<Result<List<Company>>> GetAllAsync();
        public Task<Result<Company>> GetByIdAsync(int id);
        public Task<Result<Company>> AddAsync(Company request);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result> UpdateAsync(Company request);

    }
}
