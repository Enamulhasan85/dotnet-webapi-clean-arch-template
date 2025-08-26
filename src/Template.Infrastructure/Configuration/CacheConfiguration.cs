using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Settings;

namespace Template.Infrastructure.Configuration
{
    /// <summary>
    /// Caching configuration and setup
    /// </summary>
    public static class CacheConfiguration
    {
        public static IServiceCollection AddCacheConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Get cache settings from configuration
            var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheOptions>();

            if (cacheSettings == null)
            {
                // Use default settings if not configured
                cacheSettings = new CacheOptions();
            }

            // Configure the settings for DI
            services.Configure<CacheOptions>(configuration.GetSection("CacheSettings"));

            // Configure memory cache
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = cacheSettings.MaxCacheSize;
                options.CompactionPercentage = 0.05; // Remove 5% when cache is full
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
            });

            // Note: For distributed cache (Redis), add the appropriate package and uncomment:
            // services.AddStackExchangeRedisCache(options => { ... });

            return services;
        }
    }
}
