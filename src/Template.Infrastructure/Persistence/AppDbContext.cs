using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        // Add more DbSets as needed
    }
}
