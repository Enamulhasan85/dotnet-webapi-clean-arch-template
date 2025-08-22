using FluentValidation;
using Template.Application.DTOs;

namespace Template.Application.Common.Validators
{
    public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
    {
        public CreateDoctorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required")
                .MaximumLength(100).WithMessage("Specialty must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Specialty must be at least 2 characters long");
        }
    }

    public class UpdateDoctorDtoValidator : AbstractValidator<UpdateDoctorDto>
    {
        public UpdateDoctorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters long");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("Specialty is required")
                .MaximumLength(100).WithMessage("Specialty must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Specialty must be at least 2 characters long");
        }
    }
}
