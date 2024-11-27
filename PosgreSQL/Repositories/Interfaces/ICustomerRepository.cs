using PostgreSQL.DataModels;

namespace PostgreSQL.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<Customer>> GetAllAsync();
        public Task<Customer?> GetByIdAsync(int id);
        public Task<IEnumerable<Customer>> AddAsync(Customer newCustomer);
        public Task<bool> DeleteAsync(int id);
        public Task UpdateAsync(Customer updateCustomer);
    }
}
