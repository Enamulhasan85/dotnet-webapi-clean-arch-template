namespace Template.Application.Common.Settings;

/// <summary>
/// Rate limiting configuration settings
/// </summary>
public class RateLimitSettings
{
    /// <summary>
    /// Default rate limit configuration
    /// </summary>
    public RateLimitPolicy Default { get; set; } = new();

    /// <summary>
    /// Authentication endpoints rate limit (more restrictive)
    /// </summary>
    public RateLimitPolicy Authentication { get; set; } = new();

    /// <summary>
    /// API endpoints rate limit (standard)
    /// </summary>
    public RateLimitPolicy Api { get; set; } = new();

    /// <summary>
    /// Admin endpoints rate limit (less restrictive)
    /// </summary>
    public RateLimitPolicy Admin { get; set; } = new();
}

/// <summary>
/// Rate limit policy configuration
/// </summary>
public class RateLimitPolicy
{
    /// <summary>
    /// Number of requests allowed in the time window
    /// </summary>
    public int PermitLimit { get; set; } = 100;

    /// <summary>
    /// Time window in seconds
    /// </summary>
    public int WindowInSeconds { get; set; } = 60;

    /// <summary>
    /// Number of requests that can be queued when limit is reached
    /// </summary>
    public int QueueLimit { get; set; } = 0;

    /// <summary>
    /// Queue processing order
    /// </summary>
    public string QueueProcessingOrder { get; set; } = "OldestFirst";

    /// <summary>
    /// Whether to auto-replenish the permit count
    /// </summary>
    public bool AutoReplenishment { get; set; } = true;

    /// <summary>
    /// Replenishment period in seconds
    /// </summary>
    public int ReplenishmentPeriodInSeconds { get; set; } = 1;

    /// <summary>
    /// Number of tokens to add per replenishment period
    /// </summary>
    public int TokensPerPeriod { get; set; } = 1;
}
