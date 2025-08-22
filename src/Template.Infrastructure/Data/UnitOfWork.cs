using System.Threading.Tasks;
using Template.Application.Common.Interfaces;
using Template.Infrastructure.Data.Contexts;
using Template.Infrastructure.Data.Repositories;

namespace Template.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IPatientRepository Patients { get; }
        public IDoctorRepository Doctors { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Patients = new PatientRepository(_context);
            Doctors = new DoctorRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
