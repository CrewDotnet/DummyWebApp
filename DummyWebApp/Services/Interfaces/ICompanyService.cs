using DummyWebApp.RequestModels.Company;
using DummyWebApp.ResponseModels.Company;
using FluentResults;
using PostgreSQL.DataModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface ICompanyService
    {
        public Task<CompanyResponse> GetCompanyById(int id);
        public Task<IEnumerable<CompanyResponse>> GetAllCompanies();
        public Task<CompanyResponse?> UpdateCompany(int id, UpdateCompanyRequest request);
        public Task<bool> DeleteCompany(int id);
        public Task<IEnumerable<CompanyResponse>> AddCompany(NewCompanyRequest request);


    }
}
