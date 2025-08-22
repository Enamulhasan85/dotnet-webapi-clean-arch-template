using Template.Application.Common.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Data.Contexts;

namespace Template.Infrastructure.Data.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context) { }
        // Add doctor-specific methods here
    }
}
