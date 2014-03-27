using System;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Exception called when the recovery function failed because of bad header, trailer or format.
    /// </summary>
    internal class EMVBadRecoveredDataException : Exception
    {
        /// <inheritdoc />
        public EMVBadRecoveredDataException()
            : base("Recovered data format is not as expected")
        {
        }

        /// <inheritdoc />
        public EMVBadRecoveredDataException(string message)
            : base(message)
        {
        }
    }
}