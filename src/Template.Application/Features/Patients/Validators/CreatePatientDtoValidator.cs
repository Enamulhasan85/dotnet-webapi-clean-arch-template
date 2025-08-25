using FluentValidation;
using Template.Application.Features.Patients.DTOs;
using Template.Application.Common.Validators;

namespace Template.Application.Features.Patients.Validators;

public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientDtoValidator()
    {
        RuleFor(x => x.UserProfile)
            .NotNull().WithMessage("User profile is required.")
            .SetValidator(new CreateUserProfileDtoValidator());

        When(x => !string.IsNullOrEmpty(x.BloodType), () =>
        {
            RuleFor(x => x.BloodType)
                .MaximumLength(10).WithMessage("Blood type must not exceed 10 characters.");
        });
    }
}
