using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Template.Application.Common.Settings;

namespace Template.API.Extensions
{
    /// <summary>
    /// API layer service collection extensions
    /// Contains only configurations specific to the Web API layer
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Web API specific services and configurations
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddWebApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add Authentication & Authorization (API responsibility)
            services.AddApiAuthentication(configuration);
            services.AddAuthorization();

            // Add Controllers with global filters (API responsibility)
            services.AddApiControllers();

            // Add FluentValidation (API responsibility)
            services.AddApiValidation();

            // Add AutoMapper for API layer (API responsibility)
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Add API Explorer for OpenAPI/Swagger
            services.AddEndpointsApiExplorer();

            // Add API Documentation (API responsibility)
            services.AddSwaggerServices();

            // Add API-specific services (API responsibility)
            services.AddApiServices();

            // Add CORS (API responsibility)
            services.AddApiCors();

            return services;
        }

        /// <summary>
        /// Configure JWT Authentication for API
        /// </summary>
        private static IServiceCollection AddApiAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings configuration is required");

            var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Remove default 5-minute tolerance
                };
            });

            // Configure JWT settings for DI
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            return services;
        }

        /// <summary>
        /// Configure Controllers with global filters
        /// </summary>
        private static IServiceCollection AddApiControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                // Add global exception handling
                options.Filters.Add<Template.API.Common.Filters.ExceptionFilter>();

                // Add global validation filter
                options.Filters.Add<Template.API.Common.Filters.GlobalValidationFilter>();

                // Add pagination validation with configurable max page size
                options.Filters.Add(new Template.API.Common.Filters.GlobalPaginationValidationFilter(maxPageSize: 100));
            });

            // Disable automatic model validation since we handle it globally
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        /// <summary>
        /// Configure FluentValidation for API
        /// </summary>
        private static IServiceCollection AddApiValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            return services;
        }

        /// <summary>
        /// Configure API-specific services
        /// </summary>
        private static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Register API-specific services
            services.AddScoped<Template.API.Services.IValidationService, Template.API.Services.ValidationService>();

            return services;
        }

        /// <summary>
        /// Configure CORS for API
        /// </summary>
        private static IServiceCollection AddApiCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            return services;
        }
    }
}
