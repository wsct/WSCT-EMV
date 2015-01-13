using System.Runtime.Serialization;
using WSCT.EMV.Security;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class IccCertificateData
    {
        /// <summary>
        /// RID of issuer.
        /// </summary>
        [DataMember]
        public string RegisteredIdentifier { get; set; }

        /// <summary>
        /// Application PAN.
        /// </summary>
        [DataMember]
        public string ApplicationPan { get; set; }

        /// <summary>
        /// Issuer private key.
        /// </summary>
        [DataMember]
        public PrivateKey IccPrivateKey { get; set; }

        [DataMember]
        public string HashAlgorithmIndicator { get; set; }

        [DataMember]
        public string IssuerIdentifier { get; set; }

        [DataMember]
        public string ExpirationDate { get; set; }

        [DataMember]
        public string SerialNumber { get; set; }

        [DataMember]
        public string PublicKeyAlgorithmIndicator { get; set; }
    }
}
