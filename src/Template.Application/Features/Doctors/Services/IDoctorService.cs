using Template.Application.Common.Models;
using Template.Application.Features.Doctors.DTOs;
using DoctorDtos = Template.Application.Features.Doctors.DTOs;

namespace Template.Application.Features.Doctors.Services
{
    public interface IDoctorService
    {
        Task<PaginatedResult<DoctorDtos.DoctorDto>> GetDoctorsPaginatedAsync(int pageNumber, int pageSize);
        Task<DoctorDtos.DoctorDto?> GetDoctorByIdAsync(int id);
        Task<DoctorDtos.DoctorDto> CreateDoctorAsync(DoctorDtos.CreateDoctorDto createDoctorDto);
        Task<DoctorDtos.DoctorDto?> UpdateDoctorAsync(int id, DoctorDtos.UpdateDoctorDto updateDoctorDto);
        Task<bool> DeleteDoctorAsync(int id);
    }
}
