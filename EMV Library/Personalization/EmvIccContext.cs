using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WSCT.EMV.Security;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class EmvIccContext
    {
        /// <summary>
        /// Application PAN.
        /// </summary>
        [DataMember]
        public string ApplicationPan { get; set; }

        /// <summary>
        /// ICC private key.
        /// </summary>
        [DataMember]
        public PrivateKey IccPrivateKey { get; set; }

        /// <summary>
        /// EMV certificate of ICC Public Key.
        /// </summary>
        [DataMember]
        public string IccPublicKeyCertificate { get; set; }

        /// <summary>
        /// Remainder of ICC Public Key.
        /// </summary>
        [DataMember]
        public string IccPublicKeyRemainder { get; set; }

        /// <summary>
        /// Builds the list of TlvData based on context data.
        /// </summary>
        /// <returns></returns>
        public List<TlvData> BuildTlvData()
        {
            var dictionary = new Dictionary<uint, string>
            {
                { 0x90, IccPublicKeyCertificate },
                { 0x92, IccPublicKeyRemainder }
            };
            if (IccPrivateKey != null)
            {
                dictionary.Add(0x9F32, IccPrivateKey.PublicExponent);
            }

            return dictionary
                .Where(kvp => kvp.Value != null)
                .Select(kvp => new TlvData { Tag = kvp.Key, Value = kvp.Value.FromHexa() })
                .ToList();
        }
    }
}