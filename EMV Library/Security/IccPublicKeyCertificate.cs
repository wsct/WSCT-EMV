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
        #region >> Properties

        /// <summary>
        /// Application PAN (10): PAN (padded to the right with Hex 'F's).
        /// </summary>
        public byte[] ApplicationPan { get; set; }

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

        #region >> AbstractSignatureContainer

        /// <inheritdoc />
        protected override byte[] GetDataToSign(int privateKeyLength)
        {
            DataFormat = 0x04;

            // TODO : Build data to sign
            throw new NotImplementedException();
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", DataHeader);
            s.AppendFormat("Format:[{0:X2}] ", DataFormat);
            s.AppendFormat("Issuer Identifier:[{0}] ", ApplicationPan.ToHexa('\0'));
            s.AppendFormat("Expiration:[{0}] ", CertificateExpirationDate.ToHexa('\0'));
            s.AppendFormat("Serial:[{0}] ", CertificateSerialNumber.ToHexa('\0'));
            s.AppendFormat("Hash Algorithm:[{0:X2}] ", HashAlgorithmIndicator);
            s.AppendFormat("PK Algorithm:[{0:X2}] ", PublicKeyAlgorithmIndicator);
            s.AppendFormat("PK Length:[{0:X2}] ", PublicKeyLength);
            s.AppendFormat("PKExp Length:[{0:X2}] ", PublicKeyExponentLength);
            s.AppendFormat("Leftmost IssuerPK:[{0}] ", PublicKeyorLeftmostDigitsofthePublicKey.ToHexa('\0'));
            s.AppendFormat("Hash:[{0}] ", HashResult.ToHexa('\0'));
            s.AppendFormat("Trailer:[{0:X2}] ", DataTrailer);

            return s.ToString();
        }

        #endregion
    }
}