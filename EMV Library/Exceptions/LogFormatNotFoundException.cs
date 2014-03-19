using System;

namespace WSCT.EMV.Exceptions
{
    /// <summary>
    /// Represents error that occur because the EMV data "Log Format" is undefined when needed.
    /// </summary>
    public class LogFormatNotFoundException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="LogFormatNotFoundException"/> instance.
        /// </summary>
        public LogFormatNotFoundException(String message)
            : base(message)
        {
        }
    }
}
