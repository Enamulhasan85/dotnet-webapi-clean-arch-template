using Template.Application.Common.DTOs;
using Template.Domain.Enums;

namespace Template.Application.Features.Doctors.DTOs;

public record DoctorDto
{
    public int Id { get; set; }
    public required UserProfileDto UserProfile { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public DoctorSpecialty Specialty { get; set; }
}
