using DummyWebApp.RequestModels.Company;
using DummyWebApp.ResponseModels.Company;
using PostgreSQL.DataModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface ICompanyService
    {
        public Task<CompanyResponse> GetCompanyById(int id);
        public Task<IEnumerable<CompanyResponse>> GetAllCompanies();
        public Task<CompanyResponse?> UpdateCompany(int id, UpdateCompanyRequest update);
        public Task<bool> DeleteCompany(int id);
        public Task<IEnumerable<CompanyResponse>> AddCompany(Company newCompany);


    }
}
