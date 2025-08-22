using FluentValidation;
using Template.Application.DTOs;

namespace Template.Application.Common.Validators
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past")
                .GreaterThan(DateTime.Now.AddYears(-150)).WithMessage("Date of birth cannot be more than 150 years ago");
        }
    }

    public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past")
                .GreaterThan(DateTime.Now.AddYears(-150)).WithMessage("Date of birth cannot be more than 150 years ago");
        }
    }
}
