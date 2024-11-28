using DummyWebApp.RequestModels;
using DummyWebApp.RequestModels.Company;
using DummyWebApp.ResponseModels;
using DummyWebApp.ResponseModels.Company;
using DummyWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PostgreSQL.DataModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: api/Customers
        [HttpGet]
        public Task<IEnumerable<CompanyResponse>> Get()
        {
            return _companyService.GetAllCompanies();
        }

        // GET api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyResponse>> Get(int id)
        {
            var company = await _companyService.GetCompanyById(id);
            return Ok(company);
        }

        // POST api/Company
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyResponse>>> Post([FromBody] Company newCompany)
        {
            var newCompanyList = await _companyService.AddCompany(newCompany);
            return Ok(newCompanyList);
        }

        // PUT api/Company/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyResponse>> Put(int id, [FromBody] UpdateCompanyRequest request)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var updatedCompany = await _companyService.UpdateCompany(id, request);
            return Ok(updatedCompany);
        }

        // DELETE api/Company/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var isDeleted = await _companyService.DeleteCompany(id);
            return Ok(isDeleted);
        }
    }
}
