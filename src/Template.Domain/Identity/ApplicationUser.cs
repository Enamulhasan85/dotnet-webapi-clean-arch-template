using Microsoft.AspNetCore.Identity;
using Template.Domain.Enums;

namespace Template.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void SetRefreshToken(string token, DateTime expiryTime)
    {
        RefreshToken = token;
        RefreshTokenExpiryTime = expiryTime;
    }

    public bool IsRefreshTokenValid()
    {
        return !string.IsNullOrEmpty(RefreshToken) &&
               RefreshTokenExpiryTime.HasValue &&
               RefreshTokenExpiryTime.Value > DateTime.UtcNow;
    }
}
