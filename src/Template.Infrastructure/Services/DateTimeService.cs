using System;
using Template.Application.Common.Interfaces;

namespace Template.Infrastructure.Services
{
    /// <summary>
    /// Service for handling date and time operations
    /// </summary>
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
