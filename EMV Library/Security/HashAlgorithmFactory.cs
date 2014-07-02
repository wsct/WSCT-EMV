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
        public IHashAlgorithmProvider GetProvider(byte hashIndicator)
        {
            // TODO: use an external configuration file to define hash providers
            IHashAlgorithmProvider hashProvider;
            switch (hashIndicator)
            {
                case 1:
                    hashProvider = new Sha1HashAlgorithmProvider();
                    break;
                default:
                    throw new HashAlgorithmUnknownException(hashIndicator);
            }
            return hashProvider;
        }
    }
}