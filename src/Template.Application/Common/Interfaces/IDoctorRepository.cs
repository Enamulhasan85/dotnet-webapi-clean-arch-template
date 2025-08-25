using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces;

public interface IDoctorRepository : IRepository<Doctor, int>
{
    Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyAsync(string specialty, CancellationToken cancellationToken = default);
    Task<Doctor?> GetDoctorWithUserProfileAsync(int id, CancellationToken cancellationToken = default);
}
