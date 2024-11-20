namespace DummyWebApp.Services.Interfaces.Company
{
    public interface ICompanyService
    {
        public Task<Models.Company> GetCompanyById(int id);
        public Task<IEnumerable<Models.Company>> GetAllCompanies();
        public Task<Models.Company> UpdateCompany(int id);
        public Task<Models.Company> DeleteCompany(int id);
        public Task<IEnumerable<Models.Company>> AddCompany();


    }
}
