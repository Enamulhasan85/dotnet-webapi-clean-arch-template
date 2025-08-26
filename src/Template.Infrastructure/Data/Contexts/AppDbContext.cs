using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Infrastructure.Data.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        // Add more DbSets as needed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply only App-specific entity configurations
            modelBuilder.ApplyConfiguration(new Configurations.UserProfileConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PatientConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.DoctorConfiguration());
        }
    }
}
