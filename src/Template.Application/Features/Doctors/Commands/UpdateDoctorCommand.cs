using AutoMapper;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;
using Template.Application.Features.Doctors.DTOs;
using Template.Domain.ValueObjects;

namespace Template.Application.Features.Doctors.Commands;

public class UpdateDoctorCommand : BaseCommand<Result<DoctorDto>>
{
    public required UpdateDoctorDto Doctor { get; set; }
}

public class UpdateDoctorHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDoctorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DoctorDto>> HandleAsync(UpdateDoctorCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorWithUserProfileAsync(command.Doctor.Id, cancellationToken);
            if (doctor == null)
            {
                return Result<DoctorDto>.Failure($"Doctor with ID {command.Doctor.Id} not found");
            }

            // Update UserProfile
            if (doctor.UserProfile != null)
            {
                doctor.UserProfile.FirstName = command.Doctor.UserProfile.FirstName;
                doctor.UserProfile.LastName = command.Doctor.UserProfile.LastName;
                doctor.UserProfile.Email = new Email(command.Doctor.UserProfile.Email);
                doctor.UserProfile.PhoneNumber = new PhoneNumber(command.Doctor.UserProfile.PhoneNumber);
                doctor.UserProfile.Address = command.Doctor.UserProfile.Address != null ?
                    new Address(
                        command.Doctor.UserProfile.Address.Street,
                        command.Doctor.UserProfile.Address.City,
                        command.Doctor.UserProfile.Address.State,
                        command.Doctor.UserProfile.Address.PostalCode,
                        command.Doctor.UserProfile.Address.Country
                    ) : null;
                doctor.UserProfile.DateOfBirth = command.Doctor.UserProfile.DateOfBirth;
            }

            // Update Doctor
            doctor.Specialty = command.Doctor.Specialty;
            doctor.LicenseNumber = command.Doctor.LicenseNumber;

            await _unitOfWork.Doctors.UpdateAsync(doctor, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            return Result<DoctorDto>.Success(doctorDto);
        }
        catch (Exception ex)
        {
            return Result<DoctorDto>.Failure($"Failed to update doctor: {ex.Message}");
        }
    }
}
