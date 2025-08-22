using System.ComponentModel.DataAnnotations;

namespace Template.API.Models
{
    public class CreateDoctorRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Specialty is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialty must be between 2 and 100 characters")]
        public required string Specialty { get; set; }
    }

    public class UpdateDoctorRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Specialty is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialty must be between 2 and 100 characters")]
        public required string Specialty { get; set; }
    }

    public class DoctorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
    }
}
