using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Interfaces;
using Template.Application.Services;

namespace Template.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register Application services
            services.AddScoped<DemoService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();

            return services;
        }
    }
}
