using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces;

public interface IPatientRepository : IRepository<Patient, int>
{
    Task<IEnumerable<Patient>> GetPatientsByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Patient?> GetPatientWithUserProfileAsync(int id, CancellationToken cancellationToken = default);
}
