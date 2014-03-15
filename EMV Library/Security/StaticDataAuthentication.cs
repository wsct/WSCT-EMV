using System;
using System.Text;

using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV Static Data Authentication certificate (SDA)
    /// </summary>
    public class StaticDataAuthentication : AbstractDataAuthenticationFormat03
    {
        #region >> Fields

        Byte[] _dataAuthenticationCode;

        Byte[] _padPattern;

        #endregion

        #region >> Properties

        /// <summary>
        /// Data Authentication Code (2): Issuer-assigned code
        /// </summary>
        public Byte[] dataAuthenticationCode
        {
            get
            {
                if (_dataAuthenticationCode == null)
                {
                    _dataAuthenticationCode = new Byte[2];
                    Array.Copy(_recovered, 2, _dataAuthenticationCode, 0, 2);
                }
                return _dataAuthenticationCode;
            }
        }

        /// <summary>
        /// Pad Pattern (NI - 26): (NI - 26) padding bytes of value 'BB'
        /// </summary>
        public Byte[] padPattern
        {
            get
            {
                if (_padPattern == null)
                {
                    _padPattern = new Byte[keyLength - 26];
                    Array.Copy(_recovered, 5, _padPattern, 0, keyLength - 26);
                }
                return _padPattern;
            }
        }


        #endregion

        #region >> Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public StaticDataAuthentication()
            : base()
        {
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", dataHeader);
            s.AppendFormat("Format:[{0:X2}] ", dataFormat);
            s.AppendFormat("Hash Algorithm:[{0:X2}] ", hashAlgorithmIndicator);
            s.AppendFormat("ICC Dynamic Data:[{0}] ", dataAuthenticationCode.toHexa('\0'));
            s.AppendFormat("Pad:[{0}] ", padPattern.toHexa('\0'));
            s.AppendFormat("Hash:[{0}] ", hashResult.toHexa('\0'));
            s.AppendFormat("Trailer:[{0:X2}] ", dataTrailer);

            return s.ToString();
        }
    }
}
