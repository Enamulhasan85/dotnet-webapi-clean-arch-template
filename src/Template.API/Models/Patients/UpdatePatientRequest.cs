using System;
using System.ComponentModel.DataAnnotations;

namespace Template.API.Models.Patients
{
    /// <summary>
    /// Request model for updating an existing patient
    /// </summary>
    public class UpdatePatientRequest
    {
        /// <summary>
        /// Patient's full name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public required string Name { get; set; }

        /// <summary>
        /// Patient's date of birth
        /// </summary>
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
    }
}
