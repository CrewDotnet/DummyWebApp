using DummyWebApp.Models.RequestModels.Game;
using FluentValidation;

namespace DummyWebApp.Validators.Game
{
    public class UpdateGameRequestValidator : AbstractValidator<UpdateGameRequest>
    {
        public UpdateGameRequestValidator()
        {
            RuleFor(g => g.Genre)
                .NotEmpty().WithMessage("Genre must not be empty")
                .NotNull().WithMessage("Genre must not be null")
                .Length(1, 50).WithMessage("Genre must be between 1 and 50 characters");
            RuleFor(g => g.ShortDescription)
                .NotEmpty().WithMessage("Short description must not be empty")
                .NotNull().WithMessage("Short description must not be null")
                .Length(1, 50).WithMessage("Short descriptione must be between 1 and 500 characters");
            RuleFor(g => g.Platform)
                .NotEmpty().WithMessage("Platform must not be empty")
                .NotNull().WithMessage("Platform must not be null")
                .Length(1, 50).WithMessage("Platform must be between 1 and 15 characters");
        }
    }
}

