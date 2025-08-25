using AutoMapper;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Common.Queries;
using Template.Application.Features.Doctors.DTOs;

namespace Template.Application.Features.Doctors.Queries;

public class GetDoctorQuery : BaseQuery<Result<DoctorDto>>
{
    public int Id { get; set; }
}

public class GetDoctorHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DoctorDto>> HandleAsync(GetDoctorQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorWithUserProfileAsync(query.Id, cancellationToken);
            if (doctor == null)
            {
                return Result<DoctorDto>.Failure($"Doctor with ID {query.Id} not found");
            }

            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            return Result<DoctorDto>.Success(doctorDto);
        }
        catch (Exception ex)
        {
            return Result<DoctorDto>.Failure($"Failed to get doctor: {ex.Message}");
        }
    }
}
