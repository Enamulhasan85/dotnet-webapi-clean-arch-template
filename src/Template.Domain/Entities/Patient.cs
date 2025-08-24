using Template.Domain.Common;
using Template.Domain.Enums;
using Template.Domain.ValueObjects;

namespace Template.Domain.Entities;

public class Patient : AuditableEntity<int>
{
    public int UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }

    public PatientStatus Status { get; set; } = PatientStatus.Active;
    public string? MedicalRecordNumber { get; set; }
    public string? EmergencyContact { get; set; }
    public string? BloodType { get; set; }

    public void UpdateStatus(PatientStatus newStatus)
    {
        Status = newStatus;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void UpdateMedicalInfo(string? bloodType = null, string? emergencyContact = null)
    {
        if (bloodType != null) BloodType = bloodType;
        if (emergencyContact != null) EmergencyContact = emergencyContact;
        LastModifiedAt = DateTime.UtcNow;
    }
}
