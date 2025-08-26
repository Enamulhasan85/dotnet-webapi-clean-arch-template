using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Data.Contexts;

namespace Template.Infrastructure.Data.Repositories
{
    public class PatientRepository : GenericRepository<Patient, int>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Patient>> GetPatientsByStatusAsync(string status, CancellationToken cancellationToken = default)
        {
            // Parse the string to PatientStatus enum
            if (Enum.TryParse<Domain.Enums.PatientStatus>(status, true, out var statusEnum))
            {
                return await _dbSet
                    .Where(p => p.Status == statusEnum)
                    .Include(p => p.UserProfile)
                    .ToListAsync(cancellationToken);
            }
            
            // Return empty list if status is invalid
            return new List<Patient>();
        }

        public async Task<Patient?> GetPatientWithUserProfileAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
