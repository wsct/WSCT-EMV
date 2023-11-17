using System.Runtime.Serialization;
using WSCT.EMV.Security;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class IssuerCertificateData
    {
        /// <summary>
        /// Index of Certification Authority that will sign the Issuer Public Key Certificate.
        /// </summary>
        [DataMember]
        public string CaPublicKeyIndex { get; set; }

        /// <summary>
        /// Issuer private key.
        /// </summary>
        [DataMember]
        public PrivateKey IssuerPrivateKey { get; set; }

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
