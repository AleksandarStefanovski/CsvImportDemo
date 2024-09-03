using CsvImportDemo.Models;
using FluentValidation;

namespace CsvImportDemo.Services.CsvSrvice
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(person => person.FirstName)
                .NotEmpty().WithMessage("FirstName is required.")
                .MaximumLength(50).WithMessage("FirstName must be 50 characters or fewer.");

            RuleFor(person => person.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .MaximumLength(50).WithMessage("LastName must be 50 characters or fewer.");

            RuleFor(person => person.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^\d{3}-\d{3}-\d{4}$").WithMessage("PhoneNumber must be in the format XXX-XXX-XXXX.");

            RuleFor(person => person.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(person => person.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(255).WithMessage("Address must be 255 characters or fewer.");
        }
    }
}
