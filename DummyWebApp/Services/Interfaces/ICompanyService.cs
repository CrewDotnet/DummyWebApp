using DummyWebApp.Models.RequestModels.Company;
using DummyWebApp.Models.ResponseModels.Company;
using FluentResults;
using PostgreSQL.DataModels;

namespace DummyWebApp.Services.Interfaces
{
    public interface ICompanyService
    {
        public Task<Result<CompanyResponse>> GetCompanyById(int id);
        public Task<Result<List<CompanyResponse>>> GetAllCompanies();
        public Task<Result<CompanyResponse>> UpdateCompany(int id, UpdateCompanyRequest request);
        public Task<Result<bool>> DeleteCompany(int id);
        public Task<Result<CompanyResponse>> AddCompany(NewCompanyRequest request);


    }
}
