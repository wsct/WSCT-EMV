using System;
using System.Collections.Generic;
using System.Text;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Hash algorithm provider for SHA1 method
    /// </summary>
    public class SHA1HashAlgorithmProvider: IHashAlgorithmProvider
    {
        /// <inheritdoc />
        public byte[] computeHash(List<byte[]> data)
        {
            throw new NotImplementedException();
        }
    }
}
