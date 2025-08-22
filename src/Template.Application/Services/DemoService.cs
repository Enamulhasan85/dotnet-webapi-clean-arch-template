using System.Threading.Tasks;
using Template.Application.Abstractions;
using Template.Domain.Entities;

namespace Template.Application.Services
{
    public class DemoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DemoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddPatientAndDoctorAsync()
        {
            var patient = new Patient
            {
                Name = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var doctor = new Doctor
            {
                Name = "Dr. Smith",
                Specialty = "Cardiology"
            };

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.Doctors.AddAsync(doctor);

            await _unitOfWork.CompleteAsync();
        }
    }
}
