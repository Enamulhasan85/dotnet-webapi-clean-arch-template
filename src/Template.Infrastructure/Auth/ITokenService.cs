using System.Security.Claims;
using System.Threading.Tasks;
using Template.Domain.Identity;

namespace Template.Infrastructure.Auth
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
