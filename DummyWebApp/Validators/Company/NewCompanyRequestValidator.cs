using DummyWebApp.Models.RequestModels.Company;
using FluentValidation;

namespace DummyWebApp.Validators.Company
{
    public class NewCompanyRequestValidator : AbstractValidator<NewCompanyRequest>
    {
        public NewCompanyRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");
        }
    }
}
