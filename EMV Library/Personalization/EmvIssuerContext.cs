using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class EmvIssuerContext
    {
        /// <summary>
        /// Index of Certification Authority that signed the Issuer Public Key Certificate.
        /// </summary>
        [DataMember]
        public string CaPublicKeyIndex { get; set; }

        /// <summary>
        /// Clear issuer private exponent.
        /// Mandatory for SDA only.
        /// </summary>
        [DataMember]
        public string IssuerPrivateKeyExponent { get; set; }

        [DataMember]
        public string IssuerPublicKeyModulus { get; set; }

        /// <summary>
        /// EMV certificate of Issuer Public Key.
        /// </summary>
        [DataMember]
        public string IssuerPublicKeyCertificate { get; set; }

        /// <summary>
        /// Remainder of Issuer Public Key.
        /// </summary>
        [DataMember]
        public string IssuerPublicKeyRemainder { get; set; }

        /// <summary>
        /// Exponent of Issuer Public Key.
        /// </summary>
        [DataMember]
        public string IssuerPublicKeyExponent { get; set; }

        /// <summary>
        /// Builds the list of TlvData based on context data.
        /// </summary>
        /// <returns></returns>
        public List<TlvData> BuildTlvData()
        {
            var dictionary = new Dictionary<uint, string>
            {
                { 0x8F, CaPublicKeyIndex },
                { 0x90, IssuerPublicKeyCertificate },
                { 0x92, IssuerPublicKeyRemainder },
                { 0x9F32, IssuerPublicKeyExponent }
            };

            return dictionary
                .Where(kvp => kvp.Value != null)
                .Select(kvp => new TlvData { Tag = kvp.Key, Value = kvp.Value.FromHexa() })
                .ToList();
        }
    }
}