using System;
using System.Collections.Generic;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Hash algorithm provider for SHA1 method.
    /// </summary>
    public class Sha1HashAlgorithmProvider : IHashAlgorithmProvider
    {
        #region >> IHashAlgorithmProvider

        /// <inheritdoc />
        public byte[] ComputeHash(List<byte[]> data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}