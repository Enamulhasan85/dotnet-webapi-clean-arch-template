using System.Security.Claims;

namespace Template.Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(string userId, string email, IEnumerable<string> roles, bool rememberMe = false);
    Task<string> GenerateRefreshTokenAsync();
    Task<bool> ValidateTokenAsync(string token);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
