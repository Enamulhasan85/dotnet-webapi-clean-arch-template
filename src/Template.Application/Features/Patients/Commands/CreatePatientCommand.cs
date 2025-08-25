using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Patients.DTOs;
using Template.Domain.Entities;
using Template.Domain.ValueObjects;
using AutoMapper;

namespace Template.Application.Features.Patients.Commands;

public class CreatePatientCommand : BaseCommand<Result<PatientDto>>
{
    public required CreatePatientDto Patient { get; set; }
}

public class CreatePatientHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PatientDto>> HandleAsync(CreatePatientCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Use AutoMapper to create Patient (which includes UserProfile mapping)
            var patient = _mapper.Map<Patient>(command.Patient);

            await _unitOfWork.Patients.AddAsync(patient, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var patientDto = _mapper.Map<PatientDto>(patient);
            return Result<PatientDto>.Success(patientDto);
        }
        catch (Exception ex)
        {
            return Result<PatientDto>.Failure($"Failed to create patient: {ex.Message}");
        }
    }
}
