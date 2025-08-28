using Asp.Versioning;

namespace Template.API.Extensions
{
    /// <summary>
    /// API Versioning configuration extensions
    /// </summary>
    public static class ApiVersioningExtensions
    {
        /// <summary>
        /// Configure API Versioning with all supported readers
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                // Configure API version readers (supports multiple ways to specify version)
                options.ApiVersionReader = ApiVersionReader.Combine(
                    // Query string: ?version=2.0
                    new QueryStringApiVersionReader("version"),
                    // Header: X-Version: 2.0
                    new HeaderApiVersionReader("X-Version"),
                    // URL segment: /api/v2.0/doctors
                    new UrlSegmentApiVersionReader(),
                    // Accept header: Accept: application/json;version=2.0
                    new MediaTypeApiVersionReader("version")
                );

                // Report API versions in response headers
                options.ReportApiVersions = true;
            })
            .AddMvc()
            .AddApiExplorer(setup =>
            {
                // Format for API version groups in Swagger
                setup.GroupNameFormat = "'v'VVV";

                // Replace version in URL templates
                setup.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
