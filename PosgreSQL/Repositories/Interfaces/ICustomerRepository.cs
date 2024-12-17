using FluentResults;
using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Result<List<Customer>>> GetAllAsync();
        public Task<Result<Customer>> GetByIdAsync(int id);
        public Task<Result<Customer>> AddAsync(Customer request);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result> UpdateAsync(Customer updateCustomer);
    }
}
