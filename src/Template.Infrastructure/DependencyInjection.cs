using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure.Configuration;

namespace Template.Infrastructure
{
    /// <summary>
    /// Infrastructure layer dependency injection configuration
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Add all infrastructure services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database configuration
            services.AddDatabaseConfiguration(configuration);

            // Identity configuration
            services.AddIdentityConfiguration();

            // Repository configuration
            services.AddRepositoryConfiguration();

            // Infrastructure services configuration
            services.AddInfrastructureServicesConfiguration();

            // Email configuration
            services.AddEmailConfiguration(configuration);

            // Seeding configuration
            services.AddSeedConfiguration(configuration);

            // Caching configuration
            services.AddCacheConfiguration(configuration);

            // Logging configuration
            services.AddLoggingConfiguration(configuration);

            return services;
        }
    }
}
