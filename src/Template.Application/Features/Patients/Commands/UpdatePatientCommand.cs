using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Patients.DTOs;
using AutoMapper;

namespace Template.Application.Features.Patients.Commands;

public class UpdatePatientCommand : BaseCommand<Result<PatientDto>>
{
    public required UpdatePatientDto Patient { get; set; }
}

public class UpdatePatientHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PatientDto>> HandleAsync(UpdatePatientCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(command.Patient.Id, cancellationToken);
            if (patient == null)
                return Result<PatientDto>.Failure("Patient not found");

            // Update patient properties
            patient.Status = command.Patient.Status;
            patient.BloodType = command.Patient.BloodType;
            patient.EmergencyContact = command.Patient.EmergencyContact;
            patient.MedicalRecordNumber = command.Patient.MedicalRecordNumber;

            await _unitOfWork.Patients.UpdateAsync(patient, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var patientDto = _mapper.Map<PatientDto>(patient);
            return Result<PatientDto>.Success(patientDto);
        }
        catch (Exception ex)
        {
            return Result<PatientDto>.Failure($"Failed to update patient: {ex.Message}");
        }
    }
}
