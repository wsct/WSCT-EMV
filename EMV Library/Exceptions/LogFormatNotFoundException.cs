using System;

namespace WSCT.EMV.Exceptions
{
    /// <summary>
    /// Represents error that occur because the EMV data "Log Format" is undefined when needed
    /// </summary>
    public class LogFormatNotFoundException : Exception
    {
        public LogFormatNotFoundException(String message)
            : base(message)
        {
        }
    }
}
