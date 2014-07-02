using System;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV certificate for a Public Key.
    /// </summary>
    public abstract class AbstractPublicKeyCertificate : AbstractSignatureContainer
    {
        #region >> Fields

        private readonly Int32 _identifierLength;

        #endregion

        #region >> Properties

        /// <summary>
        /// Certificate Expiration Date (2): MMYY after which this certificate is invalid.
        /// </summary>
        public byte[] CertificateExpirationDate { get; set; }

        /// <summary>
        /// Certificate Serial Number (3): Binary number unique to this certificate assigned by the certification authority.
        /// </summary>
        public byte[] CertificateSerialNumber { get; set; }

        /// <summary>
        /// Public Key (1): Identifies the digital signature algorithm to be used with the Public KeyAlgorithm Indicator.
        /// </summary>
        public byte PublicKeyAlgorithmIndicator { get; set; }

        /// <summary>
        /// Public Key Length (1): Identifies the length of the Public Key Modulus in bytes.
        /// </summary>
        public byte PublicKeyLength { get; set; }

        /// <summary>
        /// Public Key Exponent Length (1): Identifies the length of the Public Key Exponent in bytes.
        /// </summary>
        public byte PublicKeyExponentLength { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] PublicKeyorLeftmostDigitsofthePublicKey { get; set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="AbstractPublicKeyCertificate"/> instance.
        /// </summary>
        protected AbstractPublicKeyCertificate(Int32 identifierLength)
            : base(7 + identifierLength)
        {
            _identifierLength = identifierLength;
        }

        #endregion

        #region >> AbstractSignatureContainer

        /// <inheritdoc />
        protected override void OnRecoverFromSignature()
        {
            CertificateExpirationDate = new byte[2];
            Array.Copy(Recovered, 2 + _identifierLength, CertificateExpirationDate, 0, 2);

            CertificateSerialNumber = new byte[3];
            Array.Copy(Recovered, 2 + _identifierLength + 2, CertificateSerialNumber, 0, 3);

            HashAlgorithmIndicator = Recovered[2 + _identifierLength + 5];

            PublicKeyAlgorithmIndicator = Recovered[2 + _identifierLength + 6];

            PublicKeyLength = Recovered[2 + _identifierLength + 7];

            PublicKeyExponentLength = Recovered[2 + _identifierLength + 8];

            if (PublicKeyLength <= KeyLength - (22 + 10 + _identifierLength))
            {
                PublicKeyorLeftmostDigitsofthePublicKey = new byte[PublicKeyLength];
                Array.Copy(Recovered, 2 + _identifierLength + 9, PublicKeyorLeftmostDigitsofthePublicKey, 0, PublicKeyLength);
            }
            else
            {
                PublicKeyorLeftmostDigitsofthePublicKey = new byte[KeyLength - (22 + 10 + _identifierLength)];
                Array.Copy(Recovered, 2 + _identifierLength + 9, PublicKeyorLeftmostDigitsofthePublicKey, 0, KeyLength - (22 + 10 + _identifierLength));
            }
        }

        #endregion
    }
}