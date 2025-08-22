using Template.Application.DTOs;

namespace Template.Application.Common.Interfaces
{
    public interface IPatientService
    {
        Task<PaginatedResult<PatientDto>> GetPatientsPaginatedAsync(int pageNumber, int pageSize);
        Task<PatientDto?> GetPatientByIdAsync(int id);
        Task<PatientDto> CreatePatientAsync(CreatePatientDto createPatientDto);
        Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto updatePatientDto);
        Task<bool> DeletePatientAsync(int id);
    }
}
