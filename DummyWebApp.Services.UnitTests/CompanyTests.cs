using AutoFixture;
using AutoMapper;
using DummyWebApp.Models.ResponseModels.Company;
using FluentAssertions;
using FluentResults;
using Moq;
using PostgreSQL.DataModels;
using PostgreSQL.Repositories.Interfaces;

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
        public async Task GetCompanyById_ReturnsCompanyDetails_WhenCompanyExists()
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
    }
}