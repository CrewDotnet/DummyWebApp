using AutoFixture;
using DummyWebApp.Models.RequestModels.Customer;
using DummyWebApp.Validators.Customer;
using FluentValidation.TestHelper;

namespace DummyWebApp.Validation.UnitTests
{
    public class CustomerValidatorInputTests
    {
        private readonly Fixture _fixture = new();
        private readonly NewCustomerRequestValidator _newValidator;
        private readonly UpdateCustomerRequestValidator _updateValidator;

        public CustomerValidatorInputTests()
        {
            _newValidator = new NewCustomerRequestValidator();
            _updateValidator = new UpdateCustomerRequestValidator();
        }

        // NewCustomerRequest Tests
        [Fact]
        public void NewCustomerRequest_Should_HaveError_When_FirstNameIsEmpty()
        {
            // Arrange
            var model = _fixture.Build<NewCustomerRequest>()
                .With(x => x.FirstName, string.Empty)
                .Create();

            // Act
            var result = _newValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("Name must not be empty");
        }

        [Fact]
        public void NewCustomerRequest_Should_NotHaveError_When_FirstNameIsValid()
        {
            // Arrange
            var model = _fixture.Build<NewCustomerRequest>()
                .With(x => x.FirstName, "John")
                .Create();

            // Act
            var result = _newValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void NewCustomerRequest_Should_HaveError_When_EmailIsInvalid()
        {
            // Arrange
            var model = _fixture.Build<NewCustomerRequest>()
                .With(x => x.EmailAddress, "invalid-email")
                .Create();

            // Act
            var result = _newValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmailAddress)
                .WithErrorMessage("Invalid email");
        }

        // UpdateCustomerRequest Tests
        [Fact]
        public void UpdateCustomerRequest_Should_HaveError_When_LastNameIsTooShort()
        {
            // Arrange
            var model = _fixture.Build<UpdateCustomerRequest>()
                .With(x => x.LastName, "A")
                .Create();

            // Act
            var result = _updateValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name must be between 2 and 50 characters");
        }

        [Fact]
        public void UpdateCustomerRequest_Should_NotHaveError_When_LastNameIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateCustomerRequest>()
                .With(x => x.LastName, "Doe")
                .Create();

            // Act
            var result = _updateValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void UpdateCustomerRequest_Should_HaveError_When_EmailIsEmpty()
        {
            // Arrange
            var model = _fixture.Build<UpdateCustomerRequest>()
                .With(x => x.EmailAddress, string.Empty)
                .Create();

            // Act
            var result = _updateValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmailAddress)
                .WithErrorMessage("Email must not be empty");
        }

        [Fact]
        public void UpdateCustomerRequest_Should_HaveError_When_EmailIsNull()
        {
            // Arrange
            var model = _fixture.Build<UpdateCustomerRequest>()
                .With(x => x.EmailAddress, null as string)
                .Create();

            // Act
            var result = _updateValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmailAddress)
                .WithErrorMessage("Email must not be null");
        }

        [Fact]
        public void UpdateCustomerRequest_Should_NotHaveError_When_EmailIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateCustomerRequest>()
                .With(x => x.EmailAddress, "test@example.com")
                .Create();

            // Act
            var result = _updateValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.EmailAddress);
        }
    }
}

