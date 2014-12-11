using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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
