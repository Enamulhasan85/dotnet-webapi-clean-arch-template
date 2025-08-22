namespace Template.API.Models.Auth
{
    /// <summary>
    /// Request model for refreshing authentication tokens
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// The refresh token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Optional: The expired access token for additional security
        /// </summary>
        public string? AccessToken { get; set; }
    }

    /// <summary>
    /// Response model for token refresh
    /// </summary>
    public class RefreshTokenResponse
    {
        /// <summary>
        /// New JWT access token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token type (usually "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Token expiration time in seconds
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// New refresh token (optional)
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
