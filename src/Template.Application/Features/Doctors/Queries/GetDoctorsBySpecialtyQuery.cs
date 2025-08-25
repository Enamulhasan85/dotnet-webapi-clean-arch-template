using AutoMapper;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Common.Queries;
using Template.Application.Features.Doctors.DTOs;
using Template.Domain.Enums;

namespace Template.Application.Features.Doctors.Queries;

public class GetDoctorsBySpecialtyQuery : BaseQuery<Result<IEnumerable<DoctorDto>>>
{
    public DoctorSpecialty Specialty { get; set; }
}

public class GetDoctorsBySpecialtyHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoctorsBySpecialtyHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<DoctorDto>>> HandleAsync(GetDoctorsBySpecialtyQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var doctors = await _unitOfWork.Doctors.GetDoctorsBySpecialtyAsync(query.Specialty.ToString(), cancellationToken);
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);

            return Result<IEnumerable<DoctorDto>>.Success(doctorDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<DoctorDto>>.Failure($"Failed to get doctors by specialty: {ex.Message}");
        }
    }
}
