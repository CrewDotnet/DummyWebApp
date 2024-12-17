using FluentResults;
using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        public Task<Result<List<CompanyService>>> GetAllAsync();
        public Task<Result<CompanyService>> GetByIdAsync(int id);
        public Task<Result<CompanyService>> AddAsync(CompanyService request);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result> UpdateAsync(CompanyService request);

    }
}
