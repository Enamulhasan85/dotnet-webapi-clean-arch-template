namespace Template.Domain.Exceptions;

public class DoctorNotAvailableException : DomainException
{
    public int DoctorId { get; }
    public DateTime RequestedDateTime { get; }

    public DoctorNotAvailableException(int doctorId, DateTime requestedDateTime) 
        : base($"Doctor with ID {doctorId} is not available at {requestedDateTime:yyyy-MM-dd HH:mm}")
    {
        DoctorId = doctorId;
        RequestedDateTime = requestedDateTime;
    }

    public DoctorNotAvailableException(int doctorId, DateTime requestedDateTime, string reason)
        : base($"Doctor with ID {doctorId} is not available at {requestedDateTime:yyyy-MM-dd HH:mm}. Reason: {reason}")
    {
        DoctorId = doctorId;
        RequestedDateTime = requestedDateTime;
    }

    public DoctorNotAvailableException(string message) : base(message)
    {
    }

    public DoctorNotAvailableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
