using Template.Application.Common.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Data.Contexts;

namespace Template.Infrastructure.Data.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(AppDbContext context) : base(context) { }
        // Add patient-specific methods here
    }
}
