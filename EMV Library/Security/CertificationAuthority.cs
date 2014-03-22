using System;
using System.Xml.Serialization;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// EMV Authority Certification public key.
    /// </summary>
    [XmlRoot("certificationAuthority")]
    public class CertificationAuthority
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("rid")]
        public String Rid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("index")]
        public String Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("publicKey")]
        public PublicKey PublicKey { get; set; }
    }
}
