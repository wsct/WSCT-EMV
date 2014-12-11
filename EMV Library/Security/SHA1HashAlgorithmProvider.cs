using System;
using System.Collections.Generic;
using System.Linq;

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
            var length = data.Aggregate(0, (c, d) => c + d.Length);
            var bytes = new byte[length];

            var offset = 0;
            foreach (var d in data)
            {
                Array.Copy(d, 0, bytes, offset, d.Length);
                offset += d.Length;
            }

            return bytes.ComputeHashSha1();
        }

        #endregion
    }
}