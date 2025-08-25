using FluentValidation;
using Template.Application.Features.Patients.Commands;

namespace Template.Application.Features.Patients.Validators;

public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientCommandValidator()
    {
        RuleFor(x => x.Patient)
            .NotNull().WithMessage("Patient data is required.")
            .SetValidator(new CreatePatientDtoValidator());
    }
}
