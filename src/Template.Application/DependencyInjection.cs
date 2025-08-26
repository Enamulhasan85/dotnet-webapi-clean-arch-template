using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Behaviors;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Common.Services;
using Template.Application.Features.Authentication.Commands;
using Template.Application.Features.Authentication.DTOs;
using Template.Application.Features.Authentication.Queries;
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
            services.AddScoped<IDispatcher, Dispatcher>();

            // Register feature services
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();

            // Register authentication handlers with their interfaces
            services.AddScoped<ICommandHandler<LoginCommand, Result<LoginResponseDto>>, LoginHandler>();
            services.AddScoped<ICommandHandler<RegisterCommand, Result<RegisterResponseDto>>, RegisterHandler>();
            services.AddScoped<ICommandHandler<ChangePasswordCommand, Result<bool>>, ChangePasswordHandler>();
            services.AddScoped<ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResponseDto>>, RefreshTokenHandler>();
            services.AddScoped<ICommandHandler<ForgotPasswordCommand, Result<bool>>, ForgotPasswordHandler>();
            services.AddScoped<ICommandHandler<ResetPasswordCommand, Result<bool>>, ResetPasswordHandler>();
            services.AddScoped<ICommandHandler<LogoutCommand, Result<bool>>, LogoutHandler>();

            services.AddScoped<IQueryHandler<GetUserProfileQuery, Result<UserProfileResponseDto>>, GetUserProfileHandler>();

            return services;
        }
    }
}
