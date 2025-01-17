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
                .With(g => g.Title, string.Empty)
                .Create();

            // Act
            var result = _newGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.Title)
                .WithErrorMessage("Name must not be empty");
        }

        [Fact]
        public void NewGameRequest_Should_NotHaveError_When_NameIsValid()
        {
            // Arrange
            var model = _fixture.Build<NewGameRequest>()
                .With(g => g.Title, "Valid Game Name")
                .Create();

            // Act
            var result = _newGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.Title);
        }

        [Fact]
        public void UpdateGameRequest_Should_NotHaveError_When_ShortDescriptionIsNull()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.ShortDescription, (string?)null)
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.ShortDescription);
        }

        [Fact]
        public void UpdateGameRequest_Should_HaveError_When_GenreIsEmpty()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Genre, string.Empty)
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.Genre)
                .WithErrorMessage("Genre must not be empty.");
        }

        [Fact]
        public void UpdateGameRequest_Should_NotHaveError_When_GenreIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Genre, "Action")
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.Genre);
        }

        [Fact]
        public void UpdateGameRequest_Should_HaveError_When_PlatformIsEmpty()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Platform, string.Empty)
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.Platform)
                .WithErrorMessage("Platform must not be empty.");
        }

        [Fact]
        public void UpdateGameRequest_Should_NotHaveError_When_PlatformIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.Platform, "PC")
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.Platform);
        }

        [Fact]
        public void UpdateGameRequest_Should_HaveError_When_ShortDescriptionIsTooLong()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.ShortDescription, new string('A', 501)) // Assuming 500 is the max length
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(g => g.ShortDescription)
                .WithErrorMessage("ShortDescription must not exceed 500 characters.");
        }

        [Fact]
        public void UpdateGameRequest_Should_NotHaveError_When_ShortDescriptionIsValid()
        {
            // Arrange
            var model = _fixture.Build<UpdateGameRequest>()
                .With(g => g.ShortDescription, "This is a valid short description.")
                .Create();

            // Act
            var result = _updateGameValidator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(g => g.ShortDescription);
        }
    }
}

