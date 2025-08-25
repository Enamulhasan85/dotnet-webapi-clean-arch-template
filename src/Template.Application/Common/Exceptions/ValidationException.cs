namespace Template.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }

    public ValidationException(IEnumerable<string> errors) : this()
    {
        Errors = errors;
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string>();
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
        Errors = new List<string>();
    }
}
