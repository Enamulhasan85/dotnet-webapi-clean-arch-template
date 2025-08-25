using AutoMapper;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Common.Queries;
using Template.Application.Features.Patients.DTOs;

namespace Template.Application.Features.Patients.Queries;

public class GetPatientsQuery : BaseQuery<Result<PaginatedResult<PatientDto>>>
{
    public string? SearchTerm { get; set; }
}

public class GetPatientsHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<PatientDto>>> HandleAsync(GetPatientsQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            var patients = await _unitOfWork.Patients.GetPaginatedAsync(
                query.Page,
                query.PageSize,
                null, // predicate - we'll add search filtering later
                null, // orderBy
                false, // orderByDescending
                cancellationToken);

            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients.Items);

            var paginatedResult = new PaginatedResult<PatientDto>(
                patientDtos,
                patients.TotalCount,
                query.Page,
                query.PageSize);

            return Result<PaginatedResult<PatientDto>>.Success(paginatedResult);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<PatientDto>>.Failure($"Failed to get patients: {ex.Message}");
        }
    }
}
