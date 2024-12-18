﻿using DummyWebApp.Models.RequestModels.Customer;
using FluentValidation;

namespace DummyWebApp.Validators.Customer
{
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            {
                RuleFor(c => c.FirstName)
                    .NotEmpty().WithMessage("Name must not be empty")
                    .Length(2, 50).WithMessage("Name must be between 2 and 50 characters")
                    .NotNull().WithMessage("Name must not be null");
                RuleFor(c => c.LastName)
                    .NotEmpty().WithMessage("Last name must not be empty")
                    .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters")
                    .NotNull().WithMessage("Last name must not be null");
                RuleFor(c => c.EmailAddress)
                    .NotEmpty().WithMessage("Email must not be empty")
                    .NotNull().WithMessage("Email must not be null")
                    .EmailAddress().WithMessage("Invalid email");
            }
        }
    }
}
