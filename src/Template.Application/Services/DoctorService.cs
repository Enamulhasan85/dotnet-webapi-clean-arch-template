using Template.Application.Common.Interfaces;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<DoctorDto>> GetDoctorsPaginatedAsync(int pageNumber, int pageSize)
        {
            var paginatedDoctors = await _unitOfWork.Doctors.GetPagedAsync(pageNumber, pageSize);

            var doctorDtos = paginatedDoctors.Items.Select(d => new DoctorDto
            {
                Id = d.Id,
                Name = d.Name,
                Specialty = d.Specialty
            });

            return new PaginatedResult<DoctorDto>(
                doctorDtos,
                paginatedDoctors.TotalCount,
                paginatedDoctors.PageNumber,
                paginatedDoctors.PageSize);
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return null;

            return new DoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialty = doctor.Specialty
            };
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
        {
            var doctor = new Doctor
            {
                Name = createDoctorDto.Name,
                Specialty = createDoctorDto.Specialty
            };

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.CompleteAsync();

            return new DoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialty = doctor.Specialty
            };
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return null;

            doctor.Name = updateDoctorDto.Name;
            doctor.Specialty = updateDoctorDto.Specialty;

            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.CompleteAsync();

            return new DoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Specialty = doctor.Specialty
            };
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return false;

            _unitOfWork.Doctors.Delete(doctor);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
