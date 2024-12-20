using AutoFixture;
using DummyWebApp.Models.RequestModels.Game;
using DummyWebApp.Validators.Game;
using FluentValidation.TestHelper;

namespace DummyWebApp.Validation.UnitTests
{
    public class GameValidatorInputTests
    {
        private readonly Fixture _fixture = new();
        private readonly NewGameRequestValidator _newGameValidator;
        private readonly UpdateGameRequestValidator _updateGameValidator;

        public GameValidatorInputTests()
        {
            _newGameValidator = new NewGameRequestValidator();
            _updateGameValidator = new UpdateGameRequestValidator();
        }

        [Fact]
        public void NewGameRequest_Should_HaveError_When_PriceIsZero()
        {
            // Arrange
            var model = _fixture.Build<NewGameRequest>()
                .With(g => g.Price, 0)
                .Create();

            // Act
            var result = _newGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.Price)
                .WithErrorMessage("Price must be greater than 0");
        }

        [Fact]
        public void NewGameRequest_Should_NotHaveError_When_PriceIsValid()
        {
            // Arrange
            var model = _fixture.Build<NewGameRequest>()
                .With(g => g.Price, 100)
                .Create();

            // Act
            var result = _newGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.Price);
        }

        [Fact]
        public void NewGameRequest_Should_HaveError_When_NameIsEmpty()
        {
            // Arrange
            var model = _fixture.Build<NewGameRequest>()
                .With(g => g.Name, string.Empty)
                .Create();

            // Act
            var result = _newGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.Name)
                .WithErrorMessage("Name must not be empty");
        }

        [Fact]
        public void NewGameRequest_Should_NotHaveError_When_NameIsValid()
        {
            // Arrange
            var model = _fixture.Build<NewGameRequest>()
                .With(g => g.Name, "Valid Game Name")
                .Create();

            // Act
            var result = _newGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.Name);
        }

        [Fact]
        public void UpdateGameRequest_Should_HaveError_When_PriceIsNegative()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Price, -10)
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.Price)
                .WithErrorMessage("Price must be greater than 0");
        }

        [Fact]
        public void UpdateGameRequest_Should_NotHaveError_When_PriceIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Price, 150)
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.Price);
        }

        [Fact]
        public void UpdateGameRequest_Should_HaveError_When_NameIsTooLong()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Name, new string('A', 51))
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.Name)
                .WithErrorMessage("Name must be between 1 and 50 characters");
        }

        [Fact]
        public void UpdateGameRequest_Should_NotHaveError_When_NameIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Name, "Valid Name")
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.Name);
        }
    }
}

