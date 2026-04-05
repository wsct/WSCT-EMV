using System.Diagnostics.CodeAnalysis;

namespace WSCT.EMV.CommandLine.Exceptions;

public class EMVException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EMVException"/> class.
    /// </summary>
    public EMVException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EMVException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EMVException(string message) : base(message)
    {
    }

    /// <summary>
    /// Throws a <see cref="EMVException"/> if the specified argument is null.
    /// </summary>
    /// <param name="argument">The argument to check.</param>
    /// <param name="message">The message to throw with the exception.</param>
    /// <exception cref="EMVException"></exception>
    public static void ThrowIfNull([NotNull] object? argument, string message)
    {
        if (argument is null)
        {
            throw new EMVException(message);
        }
    }
}
