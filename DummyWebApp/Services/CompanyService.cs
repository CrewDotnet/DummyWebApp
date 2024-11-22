using AutoMapper;
using DummyWebApp.RequestModels;
using DummyWebApp.ResponseModels;
using DummyWebApp.Services.Interfaces.Company;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        public async Task<CompanyResponse?> UpdateCompany(int id, UpdateCompanyRequest update)
        {
            var company = await _repository.GetByIdAsync(id);
            if (company == null)
                return null;


            company.Name = update.Name;
            await _repository.UpdateAsync(company);

            return _mapper.Map<CompanyResponse>(company);

        }

        public async Task<bool> DeleteCompany(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CompanyResponse>> AddCompany(Company newCompany)
        {
            var newList = await _repository.AddAsync(newCompany);
            return _mapper.Map<IEnumerable<CompanyResponse>>(newList);
        }
    }
}
