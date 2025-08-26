using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Interfaces;
using Template.Infrastructure.Data;
using Template.Infrastructure.Data.Repositories;

namespace Template.Infrastructure.Configuration
{
    /// <summary>
    /// Repository and Unit of Work configuration
    /// </summary>
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositoryConfiguration(
            this IServiceCollection services)
        {
            // Register generic repository (if needed for direct injection)
            services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));

            // Register specific repositories
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
