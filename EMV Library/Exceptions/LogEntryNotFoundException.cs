using System;

namespace WSCT.EMV.Exceptions
{
    /// <summary>
    /// Represents error that occur because the EMV data "Log Entry" is undefined when needed.
    /// </summary>
    public class LogEntryNotFoundException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="LogEntryNotFoundException"/> instance.
        /// </summary>
        public LogEntryNotFoundException(String message)
            : base(message)
        {
        }
    }
}