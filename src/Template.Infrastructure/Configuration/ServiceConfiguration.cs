using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Interfaces;
using Template.Infrastructure.Services;

namespace Template.Infrastructure.Configuration
{
    /// <summary>
    /// Infrastructure services configuration
    /// </summary>
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddInfrastructureServicesConfiguration(
            this IServiceCollection services)
        {
            // Register infrastructure services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
