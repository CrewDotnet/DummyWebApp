using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetAllAsync();
        public Task<Company?> GetByIdAsync(int id);
        public Task<IEnumerable<Company>> AddAsync(Company newCompany);
        public Task<bool> DeleteAsync(int id);
        public Task UpdateAsync(Company newCompany);

    }
}
