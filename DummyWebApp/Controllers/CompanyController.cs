using DummyWebApp.RequestModels.Company;
using DummyWebApp.ResponseModels.Company;
using DummyWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<CompaniesDTO>>> Get()
        {
            var companies = await _companyService.GetAllCompanies();
            var response = new CompaniesDTO
            {
                Companies = companies.ToList()
            };
            return Ok(response);
        }

        // GET api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDTO>> Get(int id)
        {
            var company = await _companyService.GetCompanyById(id);
            var response = new CompanyDTO
            {
                Company = company
            };
            return Ok(response);
        }

        // POST api/Company
        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> Post([FromBody] NewCompanyRequest request)
        {
            var result = await _companyService.AddCompany(request);

            //if(!result.IsSuccess)
            //{
            //    return BadRequest(new { Errors = result.Errors.Select(e => e.Message).ToList() });
            //}
            var response = new CompanyDTO
            {
                Company = result.LastOrDefault()!
            };
            return Ok(response);
        }

        // PUT api/Company/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyDTO>> Put(int id, [FromBody] UpdateCompanyRequest request)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            var updatedCompany = await _companyService.UpdateCompany(id, request);
            if (updatedCompany == null) return BadRequest("Bad request");
            var response = new CompanyDTO
            {
                Company = updatedCompany
            };
            return Ok(response);

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
