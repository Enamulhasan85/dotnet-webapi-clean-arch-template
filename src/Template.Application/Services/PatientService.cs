using Template.Application.Common.Interfaces;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<PatientDto>> GetPatientsPaginatedAsync(int pageNumber, int pageSize)
        {
            var paginatedPatients = await _unitOfWork.Patients.GetPagedAsync(pageNumber, pageSize);

            var patientDtos = paginatedPatients.Items.Select(p => new PatientDto
            {
                Id = p.Id,
                Name = p.Name,
                DateOfBirth = p.DateOfBirth
            });

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

            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth
            };
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createPatientDto)
        {
            var patient = new Patient
            {
                Name = createPatientDto.Name,
                DateOfBirth = createPatientDto.DateOfBirth
            };

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CompleteAsync();

            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth
            };
        }

        public async Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto updatePatientDto)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null)
                return null;

            patient.Name = updatePatientDto.Name;
            patient.DateOfBirth = updatePatientDto.DateOfBirth;

            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.CompleteAsync();

            return new PatientDto
            {
                Id = patient.Id,
                Name = patient.Name,
                DateOfBirth = patient.DateOfBirth
            };
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(id);
            if (patient == null)
                return false;

            _unitOfWork.Patients.Delete(patient);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
