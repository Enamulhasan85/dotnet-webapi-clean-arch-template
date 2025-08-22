using Template.Application.Abstractions;
using Template.Domain.Entities;
using Template.Infrastructure.Persistence;

namespace Template.Infrastructure.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context) { }
        // Add patient-specific methods here
    }
}
