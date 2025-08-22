using Microsoft.Extensions.DependencyInjection;
using Template.Application.Interfaces;
using Template.Application.Services;

namespace Template.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Application-layer services here
            // Example: MediatR, AutoMapper, Validators, etc.
            // services.AddMediatR(typeof(DependencyInjection).Assembly);
            // services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Register domain services
            services.AddScoped<DemoService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();

            return services;
        }
    }
}
