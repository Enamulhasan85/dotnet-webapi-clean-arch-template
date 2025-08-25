using AutoMapper;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Doctors.DTOs;
using Template.Domain.Entities;
using Template.Domain.ValueObjects;

namespace Template.Application.Features.Doctors.Commands;

public class CreateDoctorCommand : BaseCommand<Result<DoctorDto>>
{
    public required CreateDoctorDto Doctor { get; set; }
}

public class CreateDoctorHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DoctorDto>> HandleAsync(CreateDoctorCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // Use AutoMapper to create Doctor (which includes UserProfile mapping)
            var doctor = _mapper.Map<Doctor>(command.Doctor);

            var createdDoctor = await _unitOfWork.Doctors.AddAsync(doctor, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var doctorDto = _mapper.Map<DoctorDto>(createdDoctor);
            return Result<DoctorDto>.Success(doctorDto);
        }
        catch (Exception ex)
        {
            return Result<DoctorDto>.Failure($"Failed to create doctor: {ex.Message}");
        }
    }
}
