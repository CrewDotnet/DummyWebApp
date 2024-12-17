using AutoFixture;
using AutoMapper;
using DummyWebApp.Models.ResponseModels.Company;
using FluentAssertions;
using FluentResults;
using Moq;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;
using DummyWebApp.Models.RequestModels.Company;

namespace DummyWebApp.Services.UnitTests
{
    public class CompanyTests
    {
        private readonly Mock<ICompanyRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Fixture _fixture;
        private readonly CompanyService _companyService;

        public CompanyTests()
        {
            _mockRepository = new Mock<ICompanyRepository>();
            _mockMapper = new Mock<IMapper>();
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _companyService = new CompanyService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetCompanyById_ReturnsCompanyDetails_WhenGetByIdIsSuccessful()
        {
            // Arrange
            var companyId = _fixture.Create<int>();
            var company = _fixture.Create<Company>();
            var companyResponse = _fixture.Create<CompanyResponse>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(companyId))
                .ReturnsAsync(Result.Ok(company));

            _mockMapper
                .Setup(mapper => mapper.Map<CompanyResponse>(company))
                .Returns(companyResponse);

            // Act
            var result = await _companyService.GetCompanyById(companyId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(companyResponse);

        }

        [Fact]
        public async Task GetCompanyById_ReturnsCompanyDetails_WhenGetByIdFails()
        {
            // Arrange
            var companyId = _fixture.Create<int>();
            var company = _fixture.Create<Company>();
            var companyResponse = _fixture.Create<CompanyResponse>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(companyId))
                .ReturnsAsync(Result.Fail(new Error("Company not found").WithMetadata("StatusCode", 404)));

            _mockMapper
                .Setup(mapper => mapper.Map<CompanyResponse>(company))
                .Returns(companyResponse);

            // Act
            var result = await _companyService.GetCompanyById(companyId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Company not found" && e.Metadata["StatusCode"].ToString() == "404");
        }

        [Fact]
        public async Task GetAllCompanies_ReturnsListOfAllCompanies_WhenGetAllIsSuccessful()
        {
            // Arrange
            var companies = _fixture.Create<List<Company>>();
            var companiesResponse = _fixture.Create<List<CompanyResponse>>();

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Ok(companies));

            _mockMapper
                .Setup(mapper => mapper.Map<List<CompanyResponse>>(companies))
                .Returns(companiesResponse);

            // Act
            var result = await _companyService.GetAllCompanies();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(companiesResponse);
        }
        [Fact]
        public async Task GetAllCompanies_ReturnsListOfAllCompanies_WhenGetAllFails()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(Result.Fail(new Error("No companies found in database").WithMetadata("StatusCode", 404)));

            // Act
            var result = await _companyService.GetAllCompanies();

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "No companies found in database" && e.Metadata["StatusCode"].ToString() == "404");
        }

        [Fact]
        public async Task UpdateCompany_ReturnsUpdatedCompany_WhenUpdateIsSuccessful()
        {
            // Arrange
            var companyId = _fixture.Create<int>();
            var company = _fixture.Create<Company>();
            var request = _fixture.Create<UpdateCompanyRequest>();
            var companyResponse = _fixture.Create<CompanyResponse>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(companyId))
                .ReturnsAsync(Result.Ok(company));

            _mockMapper
                .Setup(mapper => mapper.Map<CompanyResponse>(company))
                .Returns(companyResponse);

            // Act
            var result = await _companyService.UpdateCompany(companyId, request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            company.Name.Should().Be(request.Name);
            _mockRepository.Verify(repo => repo.UpdateAsync(company), Times.Once);
            result.Value.Should().Be(companyResponse);
        }

        [Fact]
        public async Task UpdateCompany_ReturnsUpdatedCompany_WhenUpdateFails()
        {
            // Arrange
            var companyId = _fixture.Create<int>();
            var request = _fixture.Create<UpdateCompanyRequest>();

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(companyId))
                .ReturnsAsync(Result.Fail(new Error("Company does not exist").WithMetadata("StatusCode", 400)));

            // Act
            var result = await _companyService.UpdateCompany(companyId, request);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Company does not exist" && e.Metadata["StatusCode"].ToString() == "400");
        }

        [Fact]
        public async Task DeleteCompany_ReturnTrue_WhenDeletionIsSuccessful()
        {
            // Arrange
            var companyId = 1;

            _mockRepository
                .Setup(repo => repo.DeleteAsync(companyId))
                .ReturnsAsync(Result.Ok(true));

            // Act
            var result = await _companyService.DeleteCompany(companyId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();
        }
        [Fact]
        public async Task DeleteCompany_ReturnsFailure_WhenDeletionFails()
        {
            // Arrange
            var companyId = 1;

            _mockRepository
                .Setup(repo => repo.DeleteAsync(companyId))
                .ReturnsAsync(Result.Fail(new Error("Deletion failed").WithMetadata("StatusCode", 400)));

            // Act
            var result = await _companyService.DeleteCompany(companyId);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Deletion failed" && e.Metadata["StatusCode"].ToString() == "400");
        }
        [Fact]
        public async Task AddCompany_ReturnsCompanyResponse_WhenSuccessful()
        {
            // Arrange
            var request = new NewCompanyRequest { Name = "New Company" };
            var companyEntity = new Company { Name = "New Company" };
            var companyResponse = new CompanyResponse { Name = "New Company" };

            _mockMapper.Setup(m => m.Map<Company>(request)).Returns(companyEntity);
            _mockMapper.Setup(m => m.Map<CompanyResponse>(companyEntity)).Returns(companyResponse);

            _mockRepository.Setup(repo => repo.AddAsync(companyEntity)).ReturnsAsync(Result.Ok(companyEntity));

            // Act
            var result = await _companyService.AddCompany(request);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(companyResponse);
        }

        [Fact]
        public async Task AddCompany_ReturnsFailure_WhenAddingCompanyFails()
        {
            // Arrange
            var request = new NewCompanyRequest { Name = "New Company" };
            var companyEntity = new Company { Name = "New Company" };

            _mockMapper.Setup(m => m.Map<Company>(request)).Returns(companyEntity);

            _mockRepository.Setup(repo => repo.AddAsync(companyEntity))
                .ReturnsAsync(Result.Fail(new Error("Failed to add company").WithMetadata("StatusCode", 500)));

            // Act
            var result = await _companyService.AddCompany(request);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e.Message == "Failed to add company");
        }
    }
}