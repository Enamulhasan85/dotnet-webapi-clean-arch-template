namespace Template.Application.Common.Interfaces;

public interface IDateTimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    DateTimeOffset OffsetNow { get; }
}
