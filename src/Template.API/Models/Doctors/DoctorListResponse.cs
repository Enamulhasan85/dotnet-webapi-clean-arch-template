using Template.API.Models.Common;

namespace Template.API.Models.Doctors
{
    /// <summary>
    /// Response model for doctor list operations
    /// </summary>
    public class DoctorListResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Summary information for quick overview
        /// </summary>
        public string Summary => $"Dr. {FullName} - {Specialization}";
    }

    /// <summary>
    /// Paginated response for doctors list
    /// </summary>
    public class DoctorsPaginatedResponse : PaginatedResponse<DoctorListResponse>
    {
        public DoctorsPaginatedResponse(IEnumerable<DoctorListResponse> items, int totalCount, int pageNumber, int pageSize)
            : base(items, totalCount, pageNumber, pageSize)
        {
        }
    }
}
