using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infrastructure.Persistence;
using Template.Domain.Identity;

namespace Template.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // User/Identity DB
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("UserConnection")));

            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<UserDbContext>();

            // Business DB
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BusinessConnection")));

            return services;
        }
    }
}
