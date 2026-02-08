using System;
using System.Text;
using WSCT.EMV.Exceptions;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV certificate for the Issuer Public Key
    /// </summary>
    public class IssuerPublicKeyCertificate : AbstractPublicKeyCertificate
    {
        #region >> Properties

        /// <summary>
        /// Issuer Identifier: Leftmost 3-8 digits from the PAN (padded to the right with Hex 'F's)
        /// </summary>
        public byte[]? IssuerIdentifier { get; set; }

        public PublicKey? IssuerPublicKey { get; set; }

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

        #region >> AbstractSignatureContainer

        /// <inheritdoc />
        protected override byte[] GetDataToSign(int privateKeyLength)
        {
            EMVApplicationException.ThrowIfNull(IssuerPublicKey, "IssuerPublicKey can't be null");
            EMVApplicationException.ThrowIfNull(IssuerPublicKey.Modulus, "IssuerPublicKey.Modulus can't be null");
            EMVApplicationException.ThrowIfNull(IssuerIdentifier, "IssuerIdentifier can't be null");
            EMVApplicationException.ThrowIfNull(CertificateExpirationDate, "CertificateExpirationDate can't be null");
            EMVApplicationException.ThrowIfNull(CertificateSerialNumber, "CertificateSerialNumber can't be null");

            var issuerPublicKeyModulus = IssuerPublicKey.Modulus.FromHexa();
            var issuerPublicKeyExponent = IssuerPublicKey.Exponent.FromHexa();

            DataFormat = 0x02;

            PublicKeyLength = (byte)issuerPublicKeyModulus.Length;
            PublicKeyExponentLength = (byte)issuerPublicKeyExponent.Length;

            PublicKeyorLeftmostDigitsofthePublicKey = new byte[privateKeyLength - 36];

            // Issuer Public Key Remainder (0 or NI – NCA + 36) - Present only if NI > NCA – 36 and consists of the NI – NCA + 36 least significant bytes of the Issuer Public Key
            byte[] issuerPublicKeyRemainder;

            if (IssuerPublicKey.Modulus.Length <= privateKeyLength - 36)
            {
                Array.Copy(issuerPublicKeyModulus, PublicKeyorLeftmostDigitsofthePublicKey, issuerPublicKeyModulus.Length);
                // pad with 'BB'
                for (var i = issuerPublicKeyModulus.Length; i < privateKeyLength - 36; i++)
                {
                    PublicKeyorLeftmostDigitsofthePublicKey[i] = 0xBB;
                }
                issuerPublicKeyRemainder = [];
            }
            else
            {
                PublicKeyorLeftmostDigitsofthePublicKey = issuerPublicKeyModulus[..(privateKeyLength - 36)];
                issuerPublicKeyRemainder = issuerPublicKeyModulus[(privateKeyLength - 36)..];
            }

            return
            [
                .. DataFormat.ToByteArray(),
                .. IssuerIdentifier,
                .. CertificateExpirationDate,
                .. CertificateSerialNumber,
                .. HashAlgorithmIndicator.ToByteArray(),
                .. PublicKeyAlgorithmIndicator.ToByteArray(),
                .. PublicKeyLength.ToByteArray(),
                .. PublicKeyExponentLength.ToByteArray(),
                .. PublicKeyorLeftmostDigitsofthePublicKey,
                .. issuerPublicKeyRemainder,
                .. issuerPublicKeyExponent,
            ];
        }

        protected override void OnRecoverFromSignature()
        {
            base.OnRecoverFromSignature();

            EMVApplicationException.ThrowIfNull(Recovered, "Recovered can't be null");

            IssuerIdentifier = new byte[4];
            Array.Copy(Recovered, 2, IssuerIdentifier, 0, 4);
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", DataHeader);
            s.AppendFormat("Format:[{0:X2}] ", DataFormat);
            s.AppendFormat("Issuer Identifier:[{0}] ", IssuerIdentifier.ToHexa('\0'));
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