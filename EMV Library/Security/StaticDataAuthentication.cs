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
        #region >> Fields

        private byte[] _dataAuthenticationCode;

        private byte[] _padPattern;

        #endregion

        #region >> Properties

        /// <summary>
        /// Data Authentication Code (2): Issuer-assigned code.
        /// </summary>
        public byte[] DataAuthenticationCode
        {
            get
            {
                if (_dataAuthenticationCode == null)
                {
                    _dataAuthenticationCode = new byte[2];
                    Array.Copy(_recovered, 2, _dataAuthenticationCode, 0, 2);
                }
                return _dataAuthenticationCode;
            }
        }

        /// <summary>
        /// Pad Pattern (NI - 26): (NI - 26) padding bytes of value 'BB'.
        /// </summary>
        public byte[] PadPattern
        {
            get
            {
                if (_padPattern == null)
                {
                    _padPattern = new byte[KeyLength - 26];
                    Array.Copy(_recovered, 5, _padPattern, 0, KeyLength - 26);
                }
                return _padPattern;
            }
        }

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
    }
}