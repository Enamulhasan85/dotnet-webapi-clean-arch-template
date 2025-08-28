using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Settings;

namespace Template.API.Extensions;

/// <summary>
/// Rate limiting configuration extensions
/// </summary>
public static class RateLimitingExtensions
{
    /// <summary>
    /// Configure rate limiting for API
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure rate limit settings
        var rateLimitSettings = configuration.GetSection("RateLimitSettings").Get<RateLimitSettings>()
            ?? new RateLimitSettings();

        services.Configure<RateLimitSettings>(configuration.GetSection("RateLimitSettings"));

        services.AddRateLimiter(options =>
        {
            // Default policy - Fixed Window
            options.AddFixedWindowLimiter("Default", fixedOptions =>
            {
                fixedOptions.PermitLimit = rateLimitSettings.Default.PermitLimit;
                fixedOptions.Window = TimeSpan.FromSeconds(rateLimitSettings.Default.WindowInSeconds);
                fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                fixedOptions.QueueLimit = rateLimitSettings.Default.QueueLimit;
            });

            // Authentication endpoints (more restrictive) - Fixed Window
            options.AddFixedWindowLimiter("Authentication", fixedOptions =>
            {
                fixedOptions.PermitLimit = rateLimitSettings.Authentication.PermitLimit;
                fixedOptions.Window = TimeSpan.FromSeconds(rateLimitSettings.Authentication.WindowInSeconds);
                fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                fixedOptions.QueueLimit = rateLimitSettings.Authentication.QueueLimit;
            });

            // API endpoints (standard) - Sliding Window
            options.AddSlidingWindowLimiter("Api", slidingOptions =>
            {
                slidingOptions.PermitLimit = rateLimitSettings.Api.PermitLimit;
                slidingOptions.Window = TimeSpan.FromSeconds(rateLimitSettings.Api.WindowInSeconds);
                slidingOptions.SegmentsPerWindow = 6;
                slidingOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                slidingOptions.QueueLimit = rateLimitSettings.Api.QueueLimit;
            });

            // Admin endpoints (less restrictive) - Token Bucket
            options.AddTokenBucketLimiter("Admin", tokenOptions =>
            {
                tokenOptions.TokenLimit = rateLimitSettings.Admin.PermitLimit;
                tokenOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                tokenOptions.QueueLimit = rateLimitSettings.Admin.QueueLimit;
                tokenOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitSettings.Admin.ReplenishmentPeriodInSeconds);
                tokenOptions.TokensPerPeriod = rateLimitSettings.Admin.TokensPerPeriod;
                tokenOptions.AutoReplenishment = rateLimitSettings.Admin.AutoReplenishment;
            });

            // Per-user rate limiting for authenticated users
            options.AddPolicy("PerUser", context =>
            {
                var userId = context.User.Identity?.Name ??
                           context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ??
                           context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: userId,
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitSettings.Api.PermitLimit * 2, // Authenticated users get more requests
                        Window = TimeSpan.FromSeconds(rateLimitSettings.Api.WindowInSeconds),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitSettings.Api.QueueLimit
                    });
            });

            // IP-based rate limiting
            options.AddPolicy("PerIP", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitSettings.Default.PermitLimit,
                        Window = TimeSpan.FromSeconds(rateLimitSettings.Default.WindowInSeconds),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = rateLimitSettings.Default.QueueLimit
                    }));

            // Global rate limiter settings
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                context => RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: "global",
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 1000, // Global limit
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 100,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(1),
                        TokensPerPeriod = 10,
                        AutoReplenishment = true
                    }));

            // Rejection response
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429; // Too Many Requests

                var response = new
                {
                    error = "Too Many Requests",
                    message = "Rate limit exceeded. Please try again later.",
                    retryAfter = TimeSpan.FromSeconds(60).TotalSeconds
                };

                await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
            };
        });

        return services;
    }
}
