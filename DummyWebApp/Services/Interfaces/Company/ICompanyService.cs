using DummyWebApp.RequestModels;
using DummyWebApp.ResponseModels;
namespace DummyWebApp.Services.Interfaces.Company
{
    public interface ICompanyService
    {
        public Task<CompanyResponse> GetCompanyById(int id);
        public Task<IEnumerable<CompanyResponse>> GetAllCompanies();
        public Task<CompanyResponse?> UpdateCompany(int id, UpdateCompanyRequest update);
        public Task<bool> DeleteCompany(int id);
        public Task<IEnumerable<CompanyResponse>> AddCompany(PostgreSQL.DataModels.Company newCompany);


    }
}
