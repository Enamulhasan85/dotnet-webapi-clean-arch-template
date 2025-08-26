using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Infrastructure.Configuration
{
    /// <summary>
    /// Logging configuration and setup
    /// </summary>
    public static class LoggingConfiguration
    {
        public static IServiceCollection AddLoggingConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Basic logging configuration
            // Additional logging providers should be configured in the API layer
            // since they often require specific packages and configuration

            return services;
        }
    }
}
