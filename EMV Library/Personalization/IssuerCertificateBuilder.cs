using System.Linq;
using Org.BouncyCastle.Math;
using WSCT.EMV.Security;
using WSCT.Helpers;

namespace WSCT.EMV.Personalization
{
    public class IssuerCertificateBuilder
    {
        private readonly IssuerCertificateData certificateData;
        private readonly PrivateKey caPrivateKey;

        #region >> Properties

        /// <summary>
        /// EMV issuer personalization context.
        /// </summary>
        public EmvIssuerContext IssuerContext { get; private set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="certificateData"></param>
        /// <param name="caPrivateKey"></param>
        public IssuerCertificateBuilder(IssuerCertificateData certificateData, PrivateKey caPrivateKey)
        {
            this.certificateData = certificateData;
            this.caPrivateKey = caPrivateKey;

            ComputeIssuerContext();
        }

        #endregion

        private void ComputeIssuerContext()
        {
            var caModulusLength = new BigInteger(caPrivateKey.Modulus, 16).BitLength / 8;
            var issuerPublicKey = certificateData.IssuerPrivateKey.GetPublicKey();
            var issuerModulusLength = new BigInteger(issuerPublicKey.Modulus, 16).BitLength / 8;

            var issuerPublicKeyCertificate = new IssuerPublicKeyCertificate
            {
                HashAlgorithmIndicator = certificateData.HashAlgorithmIndicator.FromHexa().First(),
                IssuerIdentifier = certificateData.IssuerIdentifier.FromHexa(),
                CertificateExpirationDate = certificateData.ExpirationDate.FromHexa(),
                CertificateSerialNumber = certificateData.SerialNumber.FromHexa(),
                PublicKeyAlgorithmIndicator = certificateData.PublicKeyAlgorithmIndicator.FromHexa().First(),
                IssuerPublicKey = issuerPublicKey
            };

            IssuerContext = new EmvIssuerContext()
            {
                CaPublicKeyIndex = certificateData.CaPublicKeyIndex,
                IssuerPrivateKey = certificateData.IssuerPrivateKey
            };

            // 90   Issuer Public Key Certificate (Nca)
            IssuerContext.IssuerPublicKeyCertificate = issuerPublicKeyCertificate.GenerateCertificate(caPrivateKey.GetPrivateKey()).ToHexa();

            // 92   Issuer Public Key Remainder (Ni-Nca+36)
            if (issuerModulusLength > caModulusLength - 36)
            {
                IssuerContext.IssuerPublicKeyRemainder = issuerPublicKey.Modulus.FromHexa().Skip(caModulusLength - 36).ToArray().ToHexa();
            }

            // 9F32 Issuer Public Key Exponent (1 or 3)
            IssuerContext.IssuerPrivateKey.PublicExponent = issuerPublicKey.Exponent.FromHexa().ToHexa();
        }
    }
}
