using Template.API.Models.Common;

namespace Template.API.Models.Patients
{
    /// <summary>
    /// Response model for patient list operations
    /// </summary>
    public class PatientListResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
        public string Gender { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Summary information for quick overview
        /// </summary>
        public string Summary => $"{FullName} - {Age} years old, {Gender}";
    }

    /// <summary>
    /// Paginated response for patients list
    /// </summary>
    public class PatientsPaginatedResponse : PaginatedResponse<PatientListResponse>
    {
        public PatientsPaginatedResponse(IEnumerable<PatientListResponse> items, int totalCount, int pageNumber, int pageSize)
            : base(items, totalCount, pageNumber, pageSize)
        {
        }
    }
}
