using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Template.Application.Common.Settings;

namespace Template.API.Common.Attributes
{
    /// <summary>
    /// Simple cache attribute for GET requests with configurable settings.
    /// Only caches successful (200 OK) responses with non-null values.
    /// </summary>
    public class CacheAttribute : ActionFilterAttribute
    {
        private readonly int? _durationSeconds;
        private readonly bool? _useSlidingExpiration;
        private readonly string? _keyPrefix;

        /// <summary>
        /// Uses default cache settings from configuration
        /// </summary>
        public CacheAttribute()
        {
        }

        /// <summary>
        /// Cache with custom duration in seconds
        /// </summary>
        public CacheAttribute(int durationSeconds)
        {
            _durationSeconds = durationSeconds;
        }

        /// <summary>
        /// Cache with full customization
        /// </summary>
        public CacheAttribute(int durationSeconds, bool useSlidingExpiration, string keyPrefix = "")
        {
            _durationSeconds = durationSeconds;
            _useSlidingExpiration = useSlidingExpiration;
            _keyPrefix = string.IsNullOrEmpty(keyPrefix) ? null : keyPrefix;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Only cache GET requests
            if (!IsGetRequest(context))
            {
                await next();
                return;
            }

            var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var cacheOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<CacheOptions>>().Value;
            var logger = context.HttpContext.RequestServices.GetService<ILogger<CacheAttribute>>();

            var cacheKey = GenerateCacheKey(context, cacheOptions);

            // Try to get from cache
            if (memoryCache.TryGetValue(cacheKey, out var cachedValue))
            {
                LogCacheHit(logger, cacheKey, cacheOptions.EnableLogging);
                context.Result = new ObjectResult(cachedValue) { StatusCode = 200 };
                return;
            }

            LogCacheMiss(logger, cacheKey, cacheOptions.EnableLogging);

            // Execute action
            var executedContext = await next();

            // Cache successful responses
            if (IsSuccessfulResponse(executedContext.Result))
            {
                var objectResult = (ObjectResult)executedContext.Result!;
                SetCache(memoryCache, cacheKey, objectResult.Value!, cacheOptions, logger);
            }
        }

        /// <summary>
        /// Generates a consistent cache key based on controller, action, and parameters
        /// </summary>
        private string GenerateCacheKey(ActionExecutingContext context, CacheOptions options)
        {
            var controllerName = context.Controller.GetType().Name.Replace("Controller", "");
            var actionName = context.ActionDescriptor.RouteValues["action"] ?? "Unknown";
            var keyPrefix = _keyPrefix ?? options.DefaultKeyPrefix;

            // Serialize parameters for consistent key generation
            var parameters = SerializeParameters(context.ActionArguments);

            // Create base key string
            var keyString = $"{keyPrefix}:{controllerName}:{actionName}:{parameters}";

            // Generate SHA256 hash for consistent key length and avoid special characters
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(keyString));
            var hashString = Convert.ToBase64String(hashBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            return $"{keyPrefix}:{hashString}";
        }

        /// <summary>
        /// Helper methods for cleaner code
        /// </summary>
        private static bool IsGetRequest(ActionExecutingContext context) =>
            string.Equals(context.HttpContext.Request.Method, "GET", StringComparison.OrdinalIgnoreCase);

        private static bool IsSuccessfulResponse(IActionResult? result) =>
            result is ObjectResult { StatusCode: 200, Value: not null };

        private static void LogCacheHit(ILogger<CacheAttribute>? logger, string cacheKey, bool enableLogging)
        {
            if (enableLogging && logger != null)
                logger.LogInformation("Cache HIT: {CacheKey}", cacheKey);
        }

        private static void LogCacheMiss(ILogger<CacheAttribute>? logger, string cacheKey, bool enableLogging)
        {
            if (enableLogging && logger != null)
                logger.LogInformation("Cache MISS: {CacheKey}", cacheKey);
        }

        /// <summary>
        /// Serializes action parameters to a consistent string representation
        /// </summary>
        private static string SerializeParameters(IDictionary<string, object?> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return "none";

            try
            {
                var sortedParams = parameters.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                return JsonSerializer.Serialize(sortedParams, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = false
                });
            }
            catch
            {
                // Fallback to simple concatenation
                return string.Join("_", parameters
                    .OrderBy(kvp => kvp.Key)
                    .Select(kvp => $"{kvp.Key}:{kvp.Value?.ToString() ?? "null"}"));
            }
        }

        /// <summary>
        /// Sets cache entry with configured options
        /// </summary>
        private void SetCache(IMemoryCache memoryCache, string cacheKey, object value,
            CacheOptions options, ILogger<CacheAttribute>? logger)
        {
            try
            {
                var duration = TimeSpan.FromSeconds(_durationSeconds ?? options.DefaultDurationSeconds);
                var useSlidingExpiration = _useSlidingExpiration ?? options.DefaultUseSlidingExpiration;

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    Size = options.DefaultEntrySize,
                    Priority = GetCachePriority(options.DefaultPriority)
                };

                if (useSlidingExpiration)
                    cacheEntryOptions.SlidingExpiration = duration;
                else
                    cacheEntryOptions.AbsoluteExpirationRelativeToNow = duration;

                memoryCache.Set(cacheKey, value, cacheEntryOptions);

                if (options.EnableLogging && logger != null)
                {
                    logger.LogInformation("Cache SET: {CacheKey} (Duration: {Duration}s, Sliding: {Sliding})",
                        cacheKey, duration.TotalSeconds, useSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Cache SET failed for key: {CacheKey}", cacheKey);
            }
        }

        /// <summary>
        /// Converts string priority to enum
        /// </summary>
        private static CacheItemPriority GetCachePriority(string priority) => priority?.ToLowerInvariant() switch
        {
            "low" => CacheItemPriority.Low,
            "high" => CacheItemPriority.High,
            "neverremove" => CacheItemPriority.NeverRemove,
            _ => CacheItemPriority.Normal
        };
    }
}
