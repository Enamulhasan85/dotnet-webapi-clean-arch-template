using Microsoft.EntityFrameworkCore;
using Template.API.Extensions;
using Template.Application;
using Template.Infrastructure;
using Template.Infrastructure.Persistence;

namespace Template.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebApiServices(builder.Configuration);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                // Migrate IdentityDbContext
                var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                try
                {
                    identityDbContext.Database.Migrate();
                    //IdentityDbSeeder.Seed(identityDbContext, scope.ServiceProvider).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Identity database migration failed: {ex.Message}");
                    throw;
                }

                // Migrate AppDbContext
                var appDbContext = scope.ServiceProvider.GetRequiredService<Template.Infrastructure.Persistence.AppDbContext>();
                try
                {
                    appDbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"App database migration failed: {ex.Message}");
                    throw;
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseExceptionHandler("/error");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
