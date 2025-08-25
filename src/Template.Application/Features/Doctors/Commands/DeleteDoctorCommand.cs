using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;

namespace Template.Application.Features.Doctors.Commands;

public class DeleteDoctorCommand : BaseCommand<Result>
{
    public int Id { get; set; }
}

public class DeleteDoctorHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDoctorHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteDoctorCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var doctor = await _unitOfWork.Doctors.GetByIdAsync(command.Id, cancellationToken);
            if (doctor == null)
            {
                return Result.Failure($"Doctor with ID {command.Id} not found");
            }

            await _unitOfWork.Doctors.DeleteAsync(doctor, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete doctor: {ex.Message}");
        }
    }
}
