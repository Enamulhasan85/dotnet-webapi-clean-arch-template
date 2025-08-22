using Template.Application.Abstractions;
using Template.Domain.Entities;
using Template.Infrastructure.Persistence;

namespace Template.Infrastructure.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context) { }
        // Add doctor-specific methods here
    }
}
