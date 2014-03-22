using System;
using System.Text;

using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV certificate for the ICC Public Key.
    /// </summary>
    public class IccPublicKeyCertificate : AbstractPublicKeyCertificate
    {
        #region >> Fields

        /// <summary>
        /// Application PAN (10): PAN (padded to the right with Hex 'F's) .
        /// </summary>
        private Byte[] applicationPan;

        #endregion

        #region >> Properties

        /// <summary>
        /// Application PAN (10): PAN (padded to the right with Hex 'F's).
        /// </summary>
        public Byte[] ApplicationPan
        {
            get
            {
                if (applicationPan == null)
                {
                    applicationPan = new Byte[10];
                    Array.Copy(_recovered, 2, applicationPan, 0, 10);
                }
                return applicationPan;
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="IccPublicKeyCertificate"/> instance.
        /// </summary>
        public IccPublicKeyCertificate()
            : base(10)
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
            s.AppendFormat("Issuer Identifier:[{0}] ", ApplicationPan.toHexa('\0'));
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
