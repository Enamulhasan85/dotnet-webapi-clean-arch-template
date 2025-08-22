using System;

namespace Template.Application.Common.Interfaces
{
    /// <summary>
    /// Service for handling date and time operations
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets the current UTC date and time
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets the current UTC date and time as DateTimeOffset
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
