using DummyWebApp.RequestModels;
using DummyWebApp.ResponseModels;
using PostgreSQL.DataModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>> GetAllAsync();
        public Task<CustomerResponse?> GetByIdAsync(int id);
        public Task<IEnumerable<CustomerResponse>> AddAsync(NewCustomerRequest newCustomer);
        public Task<bool> DeleteAsync(int id);
        public Task<CustomerResponse> UpdateAsync(int id , UpdateCustomerRequest request);
    }
}
