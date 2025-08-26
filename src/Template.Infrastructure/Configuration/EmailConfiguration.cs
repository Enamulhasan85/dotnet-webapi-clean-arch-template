using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Settings;

namespace Template.Infrastructure.Configuration;

/// <summary>
/// Email configuration and setup
/// </summary>
public static class EmailConfiguration
{
    public static IServiceCollection AddEmailConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure email settings
        services.Configure<EmailSettings>(
            configuration.GetSection("EmailSettings"));

        return services;
    }
}
