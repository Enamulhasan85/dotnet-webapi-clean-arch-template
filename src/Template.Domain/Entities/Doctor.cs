using Template.Domain.Common;
using Template.Domain.Enums;
using Template.Domain.ValueObjects;

namespace Template.Domain.Entities;

public class Doctor : AuditableEntity<int>
{
    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }

    public DoctorSpecialty Specialty { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
}
