using DummyWebApp.Models.RequestModels.Customer;
using DummyWebApp.Models.ResponseModels.Customer;
using FluentResults;

namespace DummyWebApp.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<Result<List<CustomerResponse>>> GetAllAsync();
        public Task<Result<CustomerResponse>> GetByIdAsync(int id);
        public Task<Result<CustomerResponse>> AddAsync(NewCustomerRequest newCustomer);
        public Task<Result<bool>> DeleteAsync(int id);
        public Task<Result<CustomerResponse>> UpdateAsync(int id , UpdateCustomerRequest request);
    }
}
