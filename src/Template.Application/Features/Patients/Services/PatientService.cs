using AutoMapper;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Patients.DTOs;
using Template.Application.Features.Patients.Services;
using Template.Domain.Entities;

namespace Template.Application.Features.Patients.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<PatientDto>> GetPatientsPaginatedAsync(int pageNumber, int pageSize)
        {
            var paginatedPatients = await _unitOfWork.Patients.GetPaginatedAsync(
                pageNumber, pageSize, null, null, false);

            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(paginatedPatients.Items);

            return new PaginatedResult<PatientDto>(
                patientDtos,
                paginatedPatients.TotalCount,
                paginatedPatients.PageNumber,
                paginatedPatients.PageSize);
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null)
                return null;

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createPatientDto)
        {
            var patient = _mapper.Map<Patient>(createPatientDto);

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto updatePatientDto)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null)
                return null;

            _mapper.Map(updatePatientDto, patient);

            await _unitOfWork.Patients.UpdateAsync(patient);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null)
                return false;

            await _unitOfWork.Patients.DeleteAsync(patient);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
