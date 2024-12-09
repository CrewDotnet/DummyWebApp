using DummyWebApp.RequestModels.Game;
using FluentValidation;
using PostgreSQL.Repositories;
using PostgreSQL.Repositories.Interfaces;

namespace DummyWebApp.Validators.Game
{
    public class NewGameRequestValidator : AbstractValidator<NewGameRequest>
    {
        public NewGameRequestValidator()
        {
            RuleFor(g => g.Price)
                .NotEmpty().WithMessage("Game must have a valid price")
                .GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(g => g.CompanyId)
                .NotEmpty().WithMessage("CompanyId is required")
                .GreaterThan(0).WithMessage("CompanyId must be greater than 0")
                .Must(g => g.GetTypeCode() == TypeCode.Int32).WithMessage("CompanyId must be integer");
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Name must not be empty")
                .NotNull().WithMessage("Name must not be null")
                .Length(1, 50).WithMessage("Name must be between 1 and 50 characters");
        }
    }
}
