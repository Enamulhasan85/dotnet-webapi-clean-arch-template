using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
