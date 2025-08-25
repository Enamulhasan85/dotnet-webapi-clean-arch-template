using Template.Application.Common.DTOs;
using Template.Domain.Enums;

namespace Template.Application.Features.Doctors.DTOs;

public record CreateDoctorDto
{
    public required CreateUserProfileDto UserProfile { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public DoctorSpecialty Specialty { get; set; }
}
