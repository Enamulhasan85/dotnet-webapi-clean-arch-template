using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Models;

namespace Template.Application.Features.Patients.Commands;

public class DeletePatientCommand : BaseCommand<Result<bool>>
{
    public int Id { get; set; }
}

public class DeletePatientHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePatientHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> HandleAsync(DeletePatientCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(command.Id, cancellationToken);
            if (patient == null)
                return Result<bool>.Failure("Patient not found");

            await _unitOfWork.Patients.DeleteAsync(patient, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to delete patient: {ex.Message}");
        }
    }
}
