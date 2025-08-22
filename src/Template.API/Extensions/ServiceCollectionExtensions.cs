using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Template.Application.Common;

namespace Template.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiServices(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            JwtSettings jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
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
                };
            });

            services.AddAuthorization();

            services.AddControllers();

            // Add FluentValidation support
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            // Register AutoMapper for API layer
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            // Register custom services
            services.AddScoped<Template.API.Services.IValidationService, Template.API.Services.ValidationService>();

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
