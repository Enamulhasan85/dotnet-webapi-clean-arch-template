using Template.Domain.Common;
using Template.Domain.Enums;
using Template.Domain.ValueObjects;

namespace Template.Domain.Entities;

public class UserProfile : AuditableEntity<int>
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Email? Email { get; set; }
    public PhoneNumber? PhoneNumber { get; set; }
    public Address? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public string FullName => $"{FirstName} {LastName}".Trim();

    public int? Age => DateOfBirth.HasValue
        ? DateTime.Today.Year - DateOfBirth.Value.Year -
          (DateTime.Today.DayOfYear < DateOfBirth.Value.DayOfYear ? 1 : 0)
        : null;

    public void UpdateContactInfo(Email? email = null, PhoneNumber? phone = null, Address? address = null)
    {
        if (email != null) Email = email;
        if (phone != null) PhoneNumber = phone;
        if (address != null) Address = address;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        LastModifiedAt = DateTime.UtcNow;
    }
}
