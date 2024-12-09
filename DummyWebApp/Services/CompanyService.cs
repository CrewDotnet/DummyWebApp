using AutoMapper;
using DummyWebApp.RequestModels.Company;
using DummyWebApp.ResponseModels.Company;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using FluentValidation;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<NewCompanyRequest> _validator;

        public CompanyService(ICompanyRepository repository, IMapper mapper, IValidator<NewCompanyRequest> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<CompanyResponse> GetCompanyById(int id)
        {
            var company = await _repository.GetByIdAsync(id);
            return _mapper.Map<CompanyResponse>(company);
        }

        public async Task<IEnumerable<CompanyResponse>> GetAllCompanies()
        {
            var allCompanies = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CompanyResponse>>(allCompanies);
        }

        public async Task<CompanyResponse?> UpdateCompany(int id, UpdateCompanyRequest request)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return null;

            company.Name = request.Name;
            await _repository.UpdateAsync(company);

            return _mapper.Map<CompanyResponse>(company);

        }

        public async Task<bool> DeleteCompany(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CompanyResponse>> AddCompany(NewCompanyRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            //if (!validationResult.IsValid)
            //{
            //    return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)).ToList());
            //}

            var mappedNewCompany = _mapper.Map<Company>(request);
            var companies = await _repository.AddAsync(mappedNewCompany);
            var companyResponses = companies.Select(company => _mapper.Map<CompanyResponse>(company));

            return companyResponses;
        }
    }
}
