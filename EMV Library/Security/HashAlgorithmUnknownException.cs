using System;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Exception called when a Hash algorithm used was not found in a <see cref="AbstractSignatureContainer"/>.
    /// </summary>
    public class HashAlgorithmUnknownException : Exception
    {
        /// <inheritdoc />
        public HashAlgorithmUnknownException()
            : base("Unknown hash algorithm used")
        {
        }

        /// <inheritdoc />
        public HashAlgorithmUnknownException(byte hashIndicator)
            : base(String.Format("Unknown hash algorithm: {0}", hashIndicator))
        {
        }
    }
}