using Template.Application.Common.Queries;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Patients.DTOs;
using AutoMapper;

namespace Template.Application.Features.Patients.Queries;

public class GetPatientQuery : BaseQuery<Result<PatientDto>>
{
    public int Id { get; set; }
}

public class GetPatientHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PatientDto>> HandleAsync(GetPatientQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(query.Id, cancellationToken);
            if (patient == null)
                return Result<PatientDto>.Failure("Patient not found");

            var patientDto = _mapper.Map<PatientDto>(patient);
            return Result<PatientDto>.Success(patientDto);
        }
        catch (Exception ex)
        {
            return Result<PatientDto>.Failure($"Failed to get patient: {ex.Message}");
        }
    }
}
