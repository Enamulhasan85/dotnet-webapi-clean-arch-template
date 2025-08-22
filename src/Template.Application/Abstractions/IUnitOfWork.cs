using System;
using System.Threading.Tasks;

namespace Template.Application.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IPatientRepository Patients { get; }
        IDoctorRepository Doctors { get; }
        Task<int> CompleteAsync();
    }
}
