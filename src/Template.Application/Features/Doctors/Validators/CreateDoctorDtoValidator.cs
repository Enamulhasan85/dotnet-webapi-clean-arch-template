using FluentValidation;
using Template.Application.Common.Validators;
using Template.Application.Features.Doctors.DTOs;

namespace Template.Application.Features.Doctors.Validators;

public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
{
    public CreateDoctorDtoValidator()
    {
        RuleFor(x => x.UserProfile)
            .NotNull().WithMessage("User profile is required.")
            .SetValidator(new CreateUserProfileDtoValidator());

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License number is required.")
            .MaximumLength(50).WithMessage("License number must not exceed 50 characters.");

        RuleFor(x => x.Specialty)
            .IsInEnum().WithMessage("Invalid specialty value.");
    }
}
