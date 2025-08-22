using Template.Application.DTOs;

namespace Template.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<PaginatedResult<DoctorDto>> GetDoctorsPaginatedAsync(int pageNumber, int pageSize);
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto);
        Task<DoctorDto?> UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto);
        Task<bool> DeleteDoctorAsync(int id);
    }
}
