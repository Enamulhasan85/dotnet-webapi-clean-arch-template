using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Behaviors;
using Template.Application.Common.Interfaces;
using Template.Application.Features.Doctors.Services;
using Template.Application.Features.Patients.Services;

namespace Template.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Register AutoMapper
            services.AddAutoMapper(assembly);

            // Register FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            // Register common services

            // Register feature services
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();

            return services;
        }
    }
}
