using FluentValidation;
using Template.Application.Features.Doctors.Queries;

namespace Template.Application.Features.Doctors.Validators;

public class GetDoctorQueryValidator : AbstractValidator<GetDoctorQuery>
{
    public GetDoctorQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than 0.");
    }
}
