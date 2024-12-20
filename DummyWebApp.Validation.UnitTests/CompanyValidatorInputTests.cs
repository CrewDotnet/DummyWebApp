using AutoFixture;
using DummyWebApp.Models.RequestModels.Company;
using DummyWebApp.Validators.Company;
using FluentValidation.TestHelper;

namespace DummyWebApp.Validation.UnitTests
{
    public class CompanyValidatorInputTests
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly NewCompanyRequestValidator _newValidator;
        private readonly UpdateCompanyRequestValidator _updateValidator;

        public CompanyValidatorInputTests()
        {
            _newValidator = new NewCompanyRequestValidator();
            _updateValidator = new UpdateCompanyRequestValidator();
        }
        [Fact]
        public void NewCompanyRequest_Should_HaveError_When_NameIsEmpty()
        {
            // Arrange
            var model = _fixture.Build<NewCompanyRequest>()
                .With(x => x.Name, string.Empty)
                .Create();

            // Act
            var result = _newValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required");
        }

        [Fact]
        public void NewCompanyRequest_Should_NotHaveError_When_NameIsValid()
        {
            // Arrange
            var model = _fixture.Build<NewCompanyRequest>()
                .With(x => x.Name, "Valid Name")
                .Create();

            // Act
            var result = _newValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void UpdateCompanyRequest_Should_HaveError_When_NameIsTooLong()
        {
            // Arrange
            var model = _fixture.Build<UpdateCompanyRequest>()
                .With(x => x.Name, new string('A', 51))
                .Create();

            // Act
            var result = _updateValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name must be between 2 and 50 characters");
        }

        [Fact]
        public void UpdateCompanyRequest_Should_NotHaveError_When_NameIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateCompanyRequest>()
                .With(x => x.Name, "Valid Company Name")
                .Create();

            // Act
            var result = _updateValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }
    }
}