using System;

namespace Template.API.Models.Patients
{
    /// <summary>
    /// Response model for patient information
    /// </summary>
    public class PatientResponse
    {
        /// <summary>
        /// Unique identifier for the patient
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Patient's full name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Patient's date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Patient's calculated age based on date of birth
        /// </summary>
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    }
}
