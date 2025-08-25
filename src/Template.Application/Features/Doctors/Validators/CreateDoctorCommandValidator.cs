using FluentValidation;
using Template.Application.Features.Doctors.Commands;

namespace Template.Application.Features.Doctors.Validators;

public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorCommandValidator()
    {
        RuleFor(x => x.Doctor)
            .NotNull().WithMessage("Doctor data is required.")
            .SetValidator(new CreateDoctorDtoValidator());
    }
}
