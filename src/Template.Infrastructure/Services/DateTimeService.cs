using System;
using Template.Application.Common.Interfaces;

namespace Template.Infrastructure.Services
{
    /// <summary>
    /// Service for handling date and time operations
    /// </summary>
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTimeOffset OffsetNow => DateTimeOffset.Now;
    }
}
