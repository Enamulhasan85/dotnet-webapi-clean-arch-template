using AutoMapper;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Common.Queries;
using Template.Application.Features.Doctors.DTOs;

namespace Template.Application.Features.Doctors.Queries;

public class GetDoctorsQuery : BaseQuery<Result<PaginatedResult<DoctorDto>>>
{
    public PaginatedRequest Request { get; set; } = new();
}

public class GetDoctorsHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoctorsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<DoctorDto>>> HandleAsync(GetDoctorsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var doctors = await _unitOfWork.Doctors.GetPaginatedAsync(
                query.Request.Page,
                query.Request.PageSize,
                predicate: null,
                orderBy: d => d.Id,
                orderByDescending: query.Request.SortDescending,
                cancellationToken);

            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors.Items);
            var result = new PaginatedResult<DoctorDto>(doctorDtos, doctors.TotalCount, doctors.PageNumber, doctors.PageSize);

            return Result<PaginatedResult<DoctorDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<DoctorDto>>.Failure($"Failed to get doctors: {ex.Message}");
        }
    }
}
