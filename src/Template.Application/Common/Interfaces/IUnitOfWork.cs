namespace Template.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDoctorRepository Doctors { get; }
    IPatientRepository Patients { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
