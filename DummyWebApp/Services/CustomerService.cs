using AutoMapper;
using DummyWebApp.Models.RequestModels.Customer;
using DummyWebApp.Models.RequestModels.Company;
using DummyWebApp.Services.Interfaces;
using FluentValidation;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;
using DummyWebApp.Models.ResponseModels.Customer;
using FluentResults;
using DummyWebApp.Models.ResponseModels.Company;

namespace DummyWebApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Result<List<CustomerResponse>>> GetAllAsync()
        {
            var getAlCustomers = await _repository.GetAllAsync();

            if (getAlCustomers.IsFailed)
            {
                return getAlCustomers.ToResult();
            }

            var result = _mapper.Map<List<CustomerResponse>>(getAlCustomers.Value);
            return Result.Ok(result);
        }

        public async Task<Result<CustomerResponse>> GetByIdAsync(int id)
        {
            var getCustomer = await _repository.GetByIdAsync(id);

            if (getCustomer.IsFailed)
            {
                return getCustomer.ToResult();
            }

            var result = _mapper.Map<CustomerResponse>(getCustomer.Value);
            return Result.Ok(result);
        }

        public async Task<Result<CustomerResponse>> AddAsync(NewCustomerRequest request)
        {
            var mappedRequest = _mapper.Map<Customer>(request);
            var result = await _repository.AddAsync(mappedRequest);

            if (result.IsFailed)
            {
                return result.ToResult();
            }

            var response = _mapper.Map<CustomerResponse>(mappedRequest);

            return Result.Ok(response);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (result.IsFailed)
            {
                return result.ToResult();
            }
            return Result.Ok(result.Value);
        }

        public async Task<Result<CustomerResponse>> UpdateAsync(int id, UpdateCustomerRequest request)
        {
            var getCustomer = await _repository.GetByIdAsync(id);

            if (getCustomer.IsFailed)
            {
                return getCustomer.ToResult();
            }

            getCustomer.Value.FirstName = request.FirstName;


            await _repository.UpdateAsync(getCustomer.Value);

            var response = _mapper.Map<CustomerResponse>(getCustomer.Value);
            return Result.Ok(response);
        }
    }
}
