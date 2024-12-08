﻿using AutoMapper;
using DummyWebApp.RequestModels.Company;
using DummyWebApp.RequestModels.Customer;
using DummyWebApp.ResponseModels.Customer;
using DummyWebApp.Services.Interfaces;
using FluentValidation;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

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
        public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerResponse>>(customers);
        }

        public async Task<CustomerResponse?> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer != null)
            {
                customer.TotalAmountSpent = customer.Games?.Sum(g => g.Price) ?? 0;
                var response = _mapper.Map<CustomerResponse>(customer);

                return response;
            }

            throw new NullReferenceException("Customer not found."); ;
        }

        public async Task<CustomerBaseResponse> AddAsync(NewCustomerRequest request)
        {
            var newCustomer = _mapper.Map<Customer>(request);
            await _repository.AddAsync(newCustomer);
            return _mapper.Map<CustomerBaseResponse> (newCustomer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<CustomerBaseResponse?> UpdateAsync(int id, UpdateCustomerRequest request)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
                return null;

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.EmailAddress = request.EmailAddress;
            await _repository.UpdateAsync(customer);

            return _mapper.Map<CustomerBaseResponse>(customer);
        }
    }
}
