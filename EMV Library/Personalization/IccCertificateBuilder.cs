using System.Linq;
using Org.BouncyCastle.Math;
using WSCT.EMV.Security;
using WSCT.Helpers;

namespace WSCT.EMV.Personalization
{
    public class IccCertificateBuilder
    {
        private readonly IccCertificateData certificateData;
        private readonly PrivateKey issuerPrivateKey;

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
            this.certificateData = certificateData;
            this.issuerPrivateKey = issuerPrivateKey;

            ComputeIccContext();
        }

        #endregion

        void ComputeIccContext()
        {
            var issuerModulusLength = new BigInteger(issuerPrivateKey.Modulus, 16).BitLength / 8;
            var iccPublicKey = certificateData.IccPrivateKey.GetPublicKey();
            var iccModulusLength = new BigInteger(iccPublicKey.Modulus, 16).BitLength / 8;

            var iccPublicKeyCertificate = new IccPublicKeyCertificate
            {
                HashAlgorithmIndicator = certificateData.HashAlgorithmIndicator.FromHexa().First(),
                ApplicationPan = certificateData.ApplicationPan.FromHexa(),
                CertificateExpirationDate = certificateData.ExpirationDate.FromHexa(),
                CertificateSerialNumber = certificateData.SerialNumber.FromHexa(),
                PublicKeyAlgorithmIndicator = certificateData.PublicKeyAlgorithmIndicator.FromHexa().First(),
                IccPublicKey = iccPublicKey
            };

            IccContext = new EmvIccContext()
            {
                ApplicationPan = certificateData.ApplicationPan,
                IccPrivateKey = certificateData.IccPrivateKey
            };

            // 9F46 ICC Public Key Certificate (Nca)
            IccContext.IccPublicKeyCertificate = iccPublicKeyCertificate.GenerateCertificate(issuerPrivateKey.GetPrivateKey()).ToHexa();

            // 9F48 ICC Public Key Remainder (Ni-Nca+42)
            if (iccModulusLength > issuerModulusLength - 42)
            {
                IccContext.IccPublicKeyRemainder = iccPublicKey.Modulus.FromHexa().Skip(issuerModulusLength - 42).ToArray().ToHexa();
            }

            // 9F47 ICC Public Key Exponent (1 or 3)
            IccContext.IccPrivateKey.PublicExponent = iccPublicKey.Exponent.FromHexa().ToHexa();
        }
    }
}
