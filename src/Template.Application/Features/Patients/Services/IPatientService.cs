using Template.Application.Common.Models;
using Template.Application.Features.Patients.DTOs;
using PatientDtos = Template.Application.Features.Patients.DTOs;

namespace Template.Application.Features.Patients.Services
{
    public interface IPatientService
    {
        Task<PaginatedResult<PatientDtos.PatientDto>> GetPatientsPaginatedAsync(int pageNumber, int pageSize);
        Task<PatientDtos.PatientDto?> GetPatientByIdAsync(int id);
        Task<PatientDtos.PatientDto> CreatePatientAsync(PatientDtos.CreatePatientDto createPatientDto);
        Task<PatientDtos.PatientDto?> UpdatePatientAsync(int id, PatientDtos.UpdatePatientDto updatePatientDto);
        Task<bool> DeletePatientAsync(int id);
    }
}
