using FluentValidation;
using Template.Application.Common.Validators;
using Template.Application.Features.Doctors.DTOs;

namespace Template.Application.Features.Doctors.Validators;

public class UpdateDoctorDtoValidator : AbstractValidator<UpdateDoctorDto>
{
    public UpdateDoctorDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than 0.");

        RuleFor(x => x.UserProfile)
            .NotNull().WithMessage("User profile is required.")
            .SetValidator(new UserProfileDtoValidator());

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License number is required.")
            .MaximumLength(50).WithMessage("License number must not exceed 50 characters.");

        RuleFor(x => x.Specialty)
            .IsInEnum().WithMessage("Invalid specialty value.");
    }
}
