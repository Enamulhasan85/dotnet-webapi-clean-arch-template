using Template.Domain.Enums;

namespace Template.Application.Common.DTOs;

public record UserProfileDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public AddressDto? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public record CreateUserProfileDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public AddressDto? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public record AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
