using AutoMapper;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Doctors.DTOs;
using Template.Application.Features.Doctors.Services;
using Template.Domain.Entities;

namespace Template.Application.Features.Doctors.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<DoctorDto>> GetDoctorsPaginatedAsync(int pageNumber, int pageSize)
        {
            var paginatedDoctors = await _unitOfWork.Doctors.GetPaginatedAsync(
                pageNumber, pageSize, null, null, false);

            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(paginatedDoctors.Items);

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

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto createDoctorDto)
        {
            var doctor = _mapper.Map<Doctor>(createDoctorDto);

            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return null;

            _mapper.Map(updateDoctorDto, doctor);

            await _unitOfWork.Doctors.UpdateAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return false;

            await _unitOfWork.Doctors.DeleteAsync(doctor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
