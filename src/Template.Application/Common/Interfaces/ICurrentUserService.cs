namespace Template.Application.Common.Interfaces
{
    /// <summary>
    /// Service for accessing current user information
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current user's ID
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets the current user's name
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Checks if the current user is authenticated
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Checks if the current user has a specific role
        /// </summary>
        /// <param name="role">The role to check</param>
        /// <returns>True if the user has the role, false otherwise</returns>
        bool IsInRole(string role);
    }
}
