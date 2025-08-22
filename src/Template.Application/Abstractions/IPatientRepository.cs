using Template.Domain.Entities;

namespace Template.Application.Abstractions
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        // Add patient-specific methods here
    }
}
