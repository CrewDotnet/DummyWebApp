using AutoMapper;
using DummyWebApp.Models.ErrorModel;
using DummyWebApp.Models.RequestModels.Company;
using DummyWebApp.Models.ResponseModels.Company;
using DummyWebApp.Presenters;
using DummyWebApp.Presenters.Company;
using DummyWebApp.Presenters.Erorr;
using DummyWebApp.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }
        // GET: api/Customers
        [HttpGet("~/api/Companies")]
        [ProducesResponseType(typeof(CompaniesDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<List<CompaniesDTO>>> Get()
        {
            var result = await _companyService.GetAllCompanies();

            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<CompaniesDTO>(result.Value);
                return CompaniesPresenter.PresentCompanies(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // GET api/CompanyService/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyDTO),200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<CompanyDTO>> Get(int id)
        {
            var result = await _companyService.GetCompanyById(id);
            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<CompanyDTO>(result.Value);
                return CompanyPresenter.PresentCompany(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // POST api/CompanyService
        [HttpPost]
        [ProducesResponseType(typeof(CompanyDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<CompanyDTO>> Post([FromBody] NewCompanyRequest request)
        {
            var result = await _companyService.AddCompany(request);

            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<CompanyDTO>(result.Value);
                return CompanyPresenter.PresentCompany(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // PUT api/CompanyService/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CompanyDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<CompanyDTO>> Put(int id, [FromBody] UpdateCompanyRequest request)
        {
            var result = await _companyService.UpdateCompany(id, request);
            if (result.IsSuccess)
            {
                var mappedResult = _mapper.Map<CompanyDTO>(result.Value);
                return CompanyPresenter.PresentCompany(mappedResult);
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }

        // DELETE api/CompanyService/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _companyService.DeleteCompany(id);
            if (result.IsSuccess)
            {
                return true;
            }

            return ErrorPresenter.PresentErrorResponse(result.Errors);
        }
    }
}
