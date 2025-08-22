namespace Template.API.Models.Doctors
{
    /// <summary>
    /// Response model for doctor information
    /// </summary>
    public class DoctorResponse
    {
        /// <summary>
        /// Unique identifier for the doctor
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Doctor's full name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Doctor's medical specialty
        /// </summary>
        public string Specialty { get; set; } = string.Empty;
    }
}
