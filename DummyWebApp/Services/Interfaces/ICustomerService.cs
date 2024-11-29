using DummyWebApp.RequestModels.Customer;
using DummyWebApp.ResponseModels.Customer;

namespace DummyWebApp.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerResponse>> GetAllAsync();
        public Task<CustomerResponse?> GetByIdAsync(int id);
        public Task<CustomerBaseResponse> AddAsync(NewCustomerRequest newCustomer);
        public Task<bool> DeleteAsync(int id);
        public Task<CustomerBaseResponse?> UpdateAsync(int id , UpdateCustomerRequest request);
    }
}
