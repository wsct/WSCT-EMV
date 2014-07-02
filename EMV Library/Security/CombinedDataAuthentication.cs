using System;

namespace WSCT.EMV.Security
{
    // TODO: implement CDA
    /// <summary>
    /// Represents an EMV Combined Data Authentication certificate (CDA).
    /// </summary>
    public class CombinedDataAuthentication : AbstractDataAuthenticationFormat05
    {
        #region >> AbstractSignatureContainer

        /// <param name="privateKeyLength"></param>
        /// <inheritdoc />
        protected override byte[] GetDataToSign(int privateKeyLength)
        {
            // TODO : Build data to sign
            throw new NotImplementedException();
        }

        #endregion
    }
}