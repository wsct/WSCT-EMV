using System.Linq;
using Org.BouncyCastle.Math;
using WSCT.EMV.Exceptions;
using WSCT.EMV.Security;
using WSCT.Helpers;

namespace WSCT.EMV.Personalization
{
    public class IccCertificateBuilder
    {
        private readonly IccCertificateData _certificateData;
        private readonly PrivateKey _issuerPrivateKey;

        #region >> Properties

        /// <summary>
        /// EMV issuer personalization context.
        /// </summary>
        public EmvIccContext IccContext { get; private set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="certificateData"></param>
        /// <param name="issuerPrivateKey"></param>
        public IccCertificateBuilder(IccCertificateData certificateData, PrivateKey issuerPrivateKey)
        {
            _certificateData = certificateData;
            _issuerPrivateKey = issuerPrivateKey;

            IccContext = ComputeIccContext();
        }

        #endregion

        EmvIccContext ComputeIccContext()
        {
            EMVApplicationException.ThrowIfNull(_certificateData, "certificateData can't be null");
            EMVApplicationException.ThrowIfNull(_certificateData.IccPrivateKey, "IccPrivateKey can't be null");
            EMVApplicationException.ThrowIfNull(_issuerPrivateKey, "issuerPrivateKey can't be null");

            var issuerModulusLength = new BigInteger(_issuerPrivateKey.Modulus, 16).BitLength / 8;
            var iccPublicKey = _certificateData.IccPrivateKey.GetPublicKey();
            var iccModulusLength = new BigInteger(iccPublicKey.Modulus, 16).BitLength / 8;

            var iccPublicKeyCertificate = new IccPublicKeyCertificate
            {
                HashAlgorithmIndicator = _certificateData.HashAlgorithmIndicator.FromHexa().First(),
                ApplicationPan = _certificateData.ApplicationPan.FromHexa(),
                CertificateExpirationDate = _certificateData.ExpirationDate.FromHexa(),
                CertificateSerialNumber = _certificateData.SerialNumber.FromHexa(),
                PublicKeyAlgorithmIndicator = _certificateData.PublicKeyAlgorithmIndicator.FromHexa().First(),
                IccPublicKey = iccPublicKey
            };

            var iccContext = new EmvIccContext
            {
                ApplicationPan = _certificateData.ApplicationPan,
                IccPrivateKey = _certificateData.IccPrivateKey,
                // 9F46 ICC Public Key Certificate (Nca)
                IccPublicKeyCertificate = iccPublicKeyCertificate.GenerateCertificate(_issuerPrivateKey.GetPrivateKey()).ToHexa()
            };

            // 9F48 ICC Public Key Remainder (Ni-Nca+42)
            if (iccModulusLength > issuerModulusLength - 42)
            {
                iccContext.IccPublicKeyRemainder = iccPublicKey.Modulus.FromHexa().Skip(issuerModulusLength - 42).ToArray().ToHexa();
            }

            // 9F47 ICC Public Key Exponent (1 or 3)
            iccContext.IccPrivateKey.PublicExponent = iccPublicKey.Exponent.FromHexa().ToHexa();

            return iccContext;
        }
    }
}
