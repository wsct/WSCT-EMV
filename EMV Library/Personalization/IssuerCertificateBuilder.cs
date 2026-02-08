using System.Linq;
using Org.BouncyCastle.Math;
using WSCT.EMV.Exceptions;
using WSCT.EMV.Security;
using WSCT.Helpers;

namespace WSCT.EMV.Personalization
{
    public class IssuerCertificateBuilder
    {
        private readonly IssuerCertificateData _certificateData;
        private readonly PrivateKey _caPrivateKey;

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
            _certificateData = certificateData;
            _caPrivateKey = caPrivateKey;

            IssuerContext = ComputeIssuerContext();
        }

        #endregion

        private EmvIssuerContext ComputeIssuerContext()
        {
            EMVApplicationException.ThrowIfNull(_certificateData, "certificateData can't be null");
            EMVApplicationException.ThrowIfNull(_certificateData.IssuerPrivateKey, "IssuerPrivateKey can't be null");
            EMVApplicationException.ThrowIfNull(_caPrivateKey, "caPrivateKey can't be null");

            var caModulusLength = new BigInteger(_caPrivateKey.Modulus, 16).BitLength / 8;
            var issuerPublicKey = _certificateData.IssuerPrivateKey.GetPublicKey();
            var issuerModulusLength = new BigInteger(issuerPublicKey.Modulus, 16).BitLength / 8;

            var issuerPublicKeyCertificate = new IssuerPublicKeyCertificate
            {
                HashAlgorithmIndicator = _certificateData.HashAlgorithmIndicator.FromHexa().First(),
                IssuerIdentifier = _certificateData.IssuerIdentifier.FromHexa(),
                CertificateExpirationDate = _certificateData.ExpirationDate.FromHexa(),
                CertificateSerialNumber = _certificateData.SerialNumber.FromHexa(),
                PublicKeyAlgorithmIndicator = _certificateData.PublicKeyAlgorithmIndicator.FromHexa().First(),
                IssuerPublicKey = issuerPublicKey
            };

            var issuerContext = new EmvIssuerContext
            {
                CaPublicKeyIndex = _certificateData.CaPublicKeyIndex,
                IssuerPrivateKey = _certificateData.IssuerPrivateKey,
                // 90   Issuer Public Key Certificate (Nca)
                IssuerPublicKeyCertificate = issuerPublicKeyCertificate.GenerateCertificate(_caPrivateKey.GetPrivateKey()).ToHexa()
            };

            // 92   Issuer Public Key Remainder (Ni-Nca+36)
            if (issuerModulusLength > caModulusLength - 36)
            {
                issuerContext.IssuerPublicKeyRemainder = issuerPublicKey.Modulus.FromHexa().Skip(caModulusLength - 36).ToArray().ToHexa();
            }

            // 9F32 Issuer Public Key Exponent (1 or 3)
            issuerContext.IssuerPrivateKey.PublicExponent = issuerPublicKey.Exponent.FromHexa().ToHexa();

            return issuerContext;
        }
    }
}
