using DummyWebApp.RequestModels.Game;
using FluentValidation;

namespace DummyWebApp.Validators.Game
{
    public class UpdateGameRequestValidator : AbstractValidator<UpdateGameRequest>
    {
        public UpdateGameRequestValidator()
        {
            RuleFor(g => g.Price)
                .NotEmpty().WithMessage("Game must have a valid price")
                .GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Name must not be empty")
                .NotNull().WithMessage("Name must not be null")
                .Length(1, 50).WithMessage("Name must be between 1 and 50 characters");
        }
    }
}
