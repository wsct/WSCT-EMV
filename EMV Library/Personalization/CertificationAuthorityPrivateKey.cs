using System.Runtime.Serialization;
using WSCT.EMV.Security;

namespace WSCT.EMV.Personalization
{
    [DataContract]
    public class CertificationAuthorityPrivate
    {
        [DataMember]
        public string Rid { get; set; }

        [DataMember]
        public string Index { get; set; }

        [DataMember]
        public PrivateKey Key { get; set; }
    }
}
