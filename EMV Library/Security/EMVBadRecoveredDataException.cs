using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Exception called when the recovery function failed because of bad header, trailer or format
    /// </summary>
    class EMVBadRecoveredDataException : Exception
    {
        /// <inheritdoc />
        public EMVBadRecoveredDataException()
            : base("Recovered data format is not as expected")
        {
        }

        /// <inheritdoc />
        public EMVBadRecoveredDataException(String message)
            : base(message)
        {
        }
    }
}
