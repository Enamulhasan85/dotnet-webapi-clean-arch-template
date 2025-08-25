namespace Template.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}
