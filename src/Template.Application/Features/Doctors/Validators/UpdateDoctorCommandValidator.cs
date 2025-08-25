using FluentValidation;
using Template.Application.Features.Doctors.Commands;

namespace Template.Application.Features.Doctors.Validators;

public class UpdateDoctorCommandValidator : AbstractValidator<UpdateDoctorCommand>
{
    public UpdateDoctorCommandValidator()
    {
        RuleFor(x => x.Doctor)
            .NotNull().WithMessage("Doctor data is required.")
            .SetValidator(new UpdateDoctorDtoValidator());
    }
}
