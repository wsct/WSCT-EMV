using System;
using System.Text;

using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV certificate for the Issuer Public Key
    /// </summary>
    public class IssuerPublicKeyCertificate : AbstractPublicKeyCertificate
    {
        #region >> Fields

        private Byte[] _issuerIdentifier;

        #endregion

        #region >> Properties

        /// <summary>
        /// Issuer Identifier: Leftmost 3-8 digits from the PAN (padded to the right with Hex 'F's)
        /// </summary>
        public Byte[] issuerIdentifier
        {
            get
            {
                if (_issuerIdentifier == null)
                {
                    _issuerIdentifier = new Byte[4];
                    Array.Copy(_recovered, 2, _issuerIdentifier, 0, 4);
                }
                return _issuerIdentifier;
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public IssuerPublicKeyCertificate()
            : base(4)
        {
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", dataHeader);
            s.AppendFormat("Format:[{0:X2}] ", dataFormat);
            s.AppendFormat("Issuer Identifier:[{0}] ", issuerIdentifier.toHexa('\0'));
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
