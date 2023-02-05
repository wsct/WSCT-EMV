namespace WSCT.EMV.Security
{
    /// <summary>
    /// Factory to use to obtain a hash algorithm provider.
    /// </summary>
    public class HashAlgorithmFactory
    {
        /// <summary>
        /// Get an instance of the hash provider defined by its <paramref name="hashIndicator"/>.
        /// </summary>
        /// <param name="hashIndicator">Hash algorithm indicator.</param>
        /// <returns>Instance of hash algorithm provider.</returns>
        public static IHashAlgorithmProvider GetProvider(byte hashIndicator)
        {
            IHashAlgorithmProvider hashProvider = hashIndicator switch
            {
                1 => new Sha1HashAlgorithmProvider(),
                _ => throw new HashAlgorithmUnknownException(hashIndicator)
            };
            return hashProvider;
        }
    }
}