namespace Template.Application.Common.Services;

public interface IAuthService
{
    Task<bool> AuthenticateAsync(string email, string password);
    Task<string> LoginAsync(string email, string password);
    Task LogoutAsync(string userId);
    Task<bool> IsUserInRoleAsync(string userId, string role);
}
