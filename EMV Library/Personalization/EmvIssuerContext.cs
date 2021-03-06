using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WSCT.EMV.Security;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.ISO7816.Commands;

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
        /// Issuer private key.
        /// </summary>
        [DataMember]
        public PrivateKey IssuerPrivateKey { get; set; }

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
        /// Builds the list of TlvData based on context data.
        /// </summary>
        /// <returns></returns>
        public List<TlvData> BuildTlvData()
        {
            var dictionary = new Dictionary<uint, string>
            {
                { 0x8F, CaPublicKeyIndex },
                { 0x90, IssuerPublicKeyCertificate },
                { 0x92, IssuerPublicKeyRemainder }
            };

            if (IssuerPrivateKey != null)
            {
                dictionary.Add(0x9F32, IssuerPrivateKey.PublicExponent);
            }

            return dictionary
                .Where(kvp => kvp.Value != null)
                .Select(kvp => new TlvData { Tag = kvp.Key, Value = kvp.Value.FromHexa() })
                .ToList();
        }
    }
}