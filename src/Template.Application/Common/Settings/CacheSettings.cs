namespace Template.Application.Common.Settings
{
    /// <summary>
    /// Configuration options for caching behavior
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// Default cache duration in seconds (default: 300 seconds = 5 minutes)
        /// </summary>
        public int DefaultDurationSeconds { get; set; } = 300;

        /// <summary>
        /// Whether to use sliding expiration by default (default: false)
        /// </summary>
        public bool DefaultUseSlidingExpiration { get; set; } = false;

        /// <summary>
        /// Default cache key prefix (default: "api_cache")
        /// </summary>
        public string DefaultKeyPrefix { get; set; } = "api_cache";

        /// <summary>
        /// Maximum number of cache entries (default: 1000)
        /// </summary>
        public int MaxCacheSize { get; set; } = 1000;

        /// <summary>
        /// Size per cache entry for size-based eviction (default: 1)
        /// </summary>
        public int DefaultEntrySize { get; set; } = 1;

        /// <summary>
        /// Enable cache logging (default: true)
        /// </summary>
        public bool EnableLogging { get; set; } = true;

        /// <summary>
        /// Cache priority for memory pressure scenarios (default: Normal)
        /// </summary>
        public string DefaultPriority { get; set; } = "Normal";
    }
}
