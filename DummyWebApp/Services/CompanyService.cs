using AutoMapper;
using DummyWebApp.Models.RequestModels.Company;
using DummyWebApp.Models.ResponseModels.Company;
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

        public CompanyService(ICompanyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<CompanyResponse>> GetCompanyById(int id)
        {
            var getCompany = await _repository.GetByIdAsync(id);

            if (getCompany.IsFailed)
            {
                return getCompany.ToResult();
            }

            var result = _mapper.Map<CompanyResponse>(getCompany.Value);
            return Result.Ok(result);
        }

        public async Task<Result<List<CompanyResponse>>> GetAllCompanies()
        {
            var getAllCompanies = await _repository.GetAllAsync();

            if (getAllCompanies.IsFailed)
            {
                return getAllCompanies.ToResult();
            }

            var result = _mapper.Map<List<CompanyResponse>>(getAllCompanies.Value);
            return Result.Ok(result);
        }

        public async Task<Result<CompanyResponse>> UpdateCompany(int id, UpdateCompanyRequest request)
        {
            var getCompany = await _repository.GetByIdAsync(id);

            if (getCompany.IsFailed)
            {
                return getCompany.ToResult();
            }

            getCompany.Value.Name = request.Name;

            await _repository.UpdateAsync(getCompany.Value);

            var response = _mapper.Map<CompanyResponse>(getCompany.Value);
            return Result.Ok(response);
        }

        public async Task<Result<bool>> DeleteCompany(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (result.IsFailed)
            {
                return result.ToResult();
            }
            return Result.Ok(result.Value);
        }

        public async Task<Result<CompanyResponse>> AddCompany(NewCompanyRequest request)
        {
            var mappedRequest = _mapper.Map<Company>(request);
            var result = await _repository.AddAsync(mappedRequest);

            if (result.IsFailed)
            {
                return result.ToResult();
            }

            var response = _mapper.Map<CompanyResponse>(mappedRequest);

            return Result.Ok(response);
        }
    }
}
