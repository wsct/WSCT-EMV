using System;
using System.Xml.Serialization;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("certificationAuthority")]
    public class CertificationAuthority
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("rid")]
        public String rid
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("index")]
        public String index
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("publicKey")]
        public PublicKey publicKey
        { get; set; }
    }
}
