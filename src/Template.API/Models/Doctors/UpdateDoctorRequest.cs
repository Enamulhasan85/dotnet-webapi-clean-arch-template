using System.ComponentModel.DataAnnotations;

namespace Template.API.Models.Doctors
{
    /// <summary>
    /// Request model for updating an existing doctor
    /// </summary>
    public class UpdateDoctorRequest
    {
        /// <summary>
        /// Doctor's full name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public required string Name { get; set; }

        /// <summary>
        /// Doctor's medical specialty
        /// </summary>
        [Required(ErrorMessage = "Specialty is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialty must be between 2 and 100 characters")]
        public required string Specialty { get; set; }
    }
}
