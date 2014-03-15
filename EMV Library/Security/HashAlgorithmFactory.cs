using System;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Factory to use to obtain a hash algorithm provider
    /// </summary>
    public class HashAlgorithmFactory
    {
        /// <summary>
        /// Get an instance of the hash provider defined by its <paramref name="hashIndicator"/>
        /// </summary>
        /// <param name="hashIndicator">Hash algorithm indicator</param>
        /// <returns>Instance of hash algorithm provider</returns>
        public IHashAlgorithmProvider getProvider(Byte hashIndicator)
        {
            // TODO: use an xml file to define hash providers
            IHashAlgorithmProvider hashProvider;
            switch (hashIndicator)
            {
                case 1:
                    hashProvider = new SHA1HashAlgorithmProvider();
                    break;
                default:
                    throw new HashAlgorithmUnknownException(hashIndicator);
            }
            return hashProvider;
        }
    }
}
