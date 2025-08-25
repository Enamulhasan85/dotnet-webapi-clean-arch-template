using Template.Application.Common.DTOs;
using Template.Domain.Enums;

namespace Template.Application.Features.Patients.DTOs;

public record PatientDto
{
    public int Id { get; set; }
    public required UserProfileDto UserProfile { get; set; }
    public PatientStatus Status { get; set; }
    public string? MedicalRecordNumber { get; set; }
    public string? EmergencyContact { get; set; }
    public string? BloodType { get; set; }
}

public record CreatePatientDto
{
    public required CreateUserProfileDto UserProfile { get; set; }
    public string? EmergencyContact { get; set; }
    public string? BloodType { get; set; }
}

public record UpdatePatientDto
{
    public int Id { get; set; }
    public required UserProfileDto UserProfile { get; set; }
    public PatientStatus Status { get; set; }
    public string? MedicalRecordNumber { get; set; }
    public string? EmergencyContact { get; set; }
    public string? BloodType { get; set; }
}
