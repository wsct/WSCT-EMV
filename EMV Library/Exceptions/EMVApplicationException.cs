using System;
using System.Diagnostics.CodeAnalysis;

namespace WSCT.EMV.Exceptions
{
    public class EMVApplicationException : Exception
    {
        public EMVApplicationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Throws a <see cref="EMVApplicationException"/> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="message">The message to throw with the exception.</param>
        /// <exception cref="EMVApplicationException"></exception>
        public static void ThrowIfNull([NotNull] object? argument, string message)
        {
            if (argument is null)
            {
                throw new EMVApplicationException(message);
            }
        }
    }
}
