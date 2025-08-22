using AutoMapper;
using Template.Application.Common.Interfaces;
using Template.Application.DTOs;
using Template.Domain.Entities;

namespace Template.Application.Services
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
            var paginatedDoctors = await _unitOfWork.Doctors.GetPagedAsync(pageNumber, pageSize);

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
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, UpdateDoctorDto updateDoctorDto)
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(id);
            if (doctor == null)
                return null;

            _mapper.Map(updateDoctorDto, doctor);

            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<DoctorDto>(doctor);
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
