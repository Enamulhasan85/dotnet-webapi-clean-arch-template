using FluentValidation;
using Template.Application.Features.Doctors.Commands;

namespace Template.Application.Features.Doctors.Validators;

public class DeleteDoctorCommandValidator : AbstractValidator<DeleteDoctorCommand>
{
    public DeleteDoctorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than 0.");
    }
}
