using System;
using System.Text;

using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV certificate for the ICC Public Key
    /// </summary>
    public class IccPublicKeyCertificate : AbstractPublicKeyCertificate
    {
        #region >> Fields

        /// <summary>
        /// Application PAN (10): PAN (padded to the right with Hex 'F's) 
        /// </summary>
        private Byte[] _applicationPAN;

        #endregion

        #region >> Properties

        /// <summary>
        /// Application PAN (10): PAN (padded to the right with Hex 'F's) 
        /// </summary>
        public Byte[] applicationPAN
        {
            get
            {
                if (_applicationPAN == null)
                {
                    _applicationPAN = new Byte[10];
                    Array.Copy(_recovered, 2, _applicationPAN, 0, 10);
                }
                return _applicationPAN;
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public IccPublicKeyCertificate()
            : base(10)
        {
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", dataHeader);
            s.AppendFormat("Format:[{0:X2}] ", dataFormat);
            s.AppendFormat("Issuer Identifier:[{0}] ", applicationPAN.toHexa('\0'));
            s.AppendFormat("Expiration:[{0}] ", certificateExpirationDate.toHexa('\0'));
            s.AppendFormat("Serial:[{0}] ", certificateSerialNumber.toHexa('\0'));
            s.AppendFormat("Hash Algorithm:[{0:X2}] ", hashAlgorithmIndicator);
            s.AppendFormat("PK Algorithm:[{0:X2}] ", publicKeyAlgorithmIndicator);
            s.AppendFormat("PK Length:[{0:X2}] ", publicKeyLength);
            s.AppendFormat("PKExp Length:[{0:X2}] ", publicKeyExponentLength);
            s.AppendFormat("Leftmost IssuerPK:[{0}] ", publicKeyorLeftmostDigitsofthePublicKey.toHexa('\0'));
            s.AppendFormat("Hash:[{0}] ", hashResult.toHexa('\0'));
            s.AppendFormat("Trailer:[{0:X2}] ", dataTrailer);

            return s.ToString();
        }

    }
}
