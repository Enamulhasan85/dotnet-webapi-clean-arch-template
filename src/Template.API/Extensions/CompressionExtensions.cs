using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace Template.API.Extensions;

/// <summary>
/// Response compression configuration extensions
/// </summary>
public static class CompressionExtensions
{
    /// <summary>
    /// Configure response compression for API with sane defaults
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            // Security: Disable HTTPS compression by default to prevent BREACH attacks
            options.EnableForHttps = true;

            // Add both Gzip and Brotli providers for best client support
            options.Providers.Add<GzipCompressionProvider>();
            options.Providers.Add<BrotliCompressionProvider>();

            // Use default MIME types plus JSON/XML for APIs
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                "application/json",
                "application/xml"
            });
        });

        // Configure compression levels for optimal performance
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        return services;
    }
}
