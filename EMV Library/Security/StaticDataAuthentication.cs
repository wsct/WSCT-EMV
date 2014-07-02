using System;
using System.Text;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV Static Data Authentication certificate (SDA).
    /// </summary>
    public class StaticDataAuthentication : AbstractDataAuthenticationFormat03
    {
        #region >> Properties

        /// <summary>
        /// Data Authentication Code (2): Issuer-assigned code.
        /// </summary>
        public byte[] DataAuthenticationCode { get; set; }

        /// <summary>
        /// Pad Pattern (NI - 26): (NI - 26) padding bytes of value 'BB'.
        /// </summary>
        public byte[] PadPattern { get; set; }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", DataHeader);
            s.AppendFormat("Format:[{0:X2}] ", DataFormat);
            s.AppendFormat("Hash Algorithm:[{0:X2}] ", HashAlgorithmIndicator);
            s.AppendFormat("ICC Dynamic Data:[{0}] ", DataAuthenticationCode.ToHexa('\0'));
            s.AppendFormat("Pad:[{0}] ", PadPattern.ToHexa('\0'));
            s.AppendFormat("Hash:[{0}] ", HashResult.ToHexa('\0'));
            s.AppendFormat("Trailer:[{0:X2}] ", DataTrailer);

            return s.ToString();
        }

        #endregion

        #region >> AbstractSignatureContainer

        /// <param name="privateKeyLength"></param>
        /// <inheritdoc />
        protected override byte[] GetDataToSign(int privateKeyLength)
        {
            // TODO : Build data to sign
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OnRecoverFromSignature()
        {
            DataAuthenticationCode = new byte[2];
            Array.Copy(Recovered, 2, DataAuthenticationCode, 0, 2);

            PadPattern = new byte[KeyLength - 26];
            Array.Copy(Recovered, 5, PadPattern, 0, KeyLength - 26);
        }

        #endregion
    }
}