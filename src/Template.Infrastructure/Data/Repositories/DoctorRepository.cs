using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Data.Contexts;

namespace Template.Infrastructure.Data.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor, int>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default)
        {
            // Parse the string to DoctorSpecialty enum
            if (Enum.TryParse<Domain.Enums.DoctorSpecialty>(specialty, true, out var specialtyEnum))
            {
                return await _dbSet
                    .Where(d => d.Specialty == specialtyEnum)
                    .Include(d => d.UserProfile)
                    .ToListAsync(cancellationToken);
            }
            
            // Return empty list if specialty is invalid
            return new List<Doctor>();
        }

        public async Task<Doctor?> GetDoctorWithUserProfileAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(d => d.UserProfile)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }
    }
}
