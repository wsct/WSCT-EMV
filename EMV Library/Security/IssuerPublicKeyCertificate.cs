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

        private byte[] _issuerIdentifier;

        #endregion

        #region >> Properties

        /// <summary>
        /// Issuer Identifier: Leftmost 3-8 digits from the PAN (padded to the right with Hex 'F's)
        /// </summary>
        public Byte[] IssuerIdentifier
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

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", DataHeader);
            s.AppendFormat("Format:[{0:X2}] ", DataFormat);
            s.AppendFormat("Issuer Identifier:[{0}] ", IssuerIdentifier.toHexa('\0'));
            s.AppendFormat("Expiration:[{0}] ", CertificateExpirationDate.toHexa('\0'));
            s.AppendFormat("Serial:[{0}] ", CertificateSerialNumber.toHexa('\0'));
            s.AppendFormat("Hash Algorithm:[{0:X2}] ", HashAlgorithmIndicator);
            s.AppendFormat("PK Algorithm:[{0:X2}] ", PublicKeyAlgorithmIndicator);
            s.AppendFormat("PK Length:[{0:X2}] ", PublicKeyLength);
            s.AppendFormat("PKExp Length:[{0:X2}] ", PublicKeyExponentLength);
            s.AppendFormat("Leftmost IssuerPK:[{0}] ", PublicKeyorLeftmostDigitsofthePublicKey.toHexa('\0'));
            s.AppendFormat("Hash:[{0}] ", HashResult.toHexa('\0'));
            s.AppendFormat("Trailer:[{0:X2}] ", DataTrailer);

            return s.ToString();
        }

        #endregion
    }
}
