using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.EMV.Security;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    public class TerminalParameters : WSCT.Helpers.Xml.IXmlSerializable
    {
        #region >> Fields

        List<String> _knownAIDs;
        CertificationAuthorityRepository _certificationAuthorityRepository;

        #endregion

        #region >> Properties

        public List<String> knownAIDs
        {
            get { return _knownAIDs; }
        }
        public CertificationAuthorityRepository certificationAuthorityRepository
        {
            get { return _certificationAuthorityRepository; }
            set { _certificationAuthorityRepository = value; }
        }

        #endregion

        #region >> Constructors

        public TerminalParameters()
        {
            _knownAIDs = new List<string>();
        }

        #endregion

        #region >> IXmlSerializable Members

        /// <inheritdoc />
        public void fromXmlNode(System.Xml.XmlNode xmlNode, System.Xml.XmlNamespaceManager xmlNamespace)
        {
            // Loads applications list supported by the terminal
            foreach (System.Xml.XmlNode xmlSubNode in xmlNode.SelectNodes("ns:capabilities/ns:supportedApplications/ns:application", xmlNamespace))
            {
                foreach (System.Xml.XmlAttribute xmlAttr in xmlSubNode.Attributes)
                {
                    switch (xmlAttr.Name)
                    {
                        case "aid":
                            _knownAIDs.Add(xmlAttr.Value);
                            break;
                        case "label":
                            // TODO: something with it
                            break;
                        default:
                            break;
                    }
                }
            }

            // Loads certification authorities list supported by the terminal
            certificationAuthorityRepository = new CertificationAuthorityRepository();

            foreach (System.Xml.XmlNode xmlSubNode in xmlNode.SelectNodes("ns:certificationAuthorities/ns:certificationAuthority", xmlNamespace))
            {
                String rid = "";
                String index = "";
                foreach (System.Xml.XmlAttribute xmlAttr in xmlSubNode.Attributes)
                {
                    switch (xmlAttr.Name)
                    {
                        case "rid":
                            rid = xmlAttr.Value;
                            break;
                        case "index":
                            index = xmlAttr.Value;
                            break;
                        default:
                            break;
                    }
                }

                PublicKey publicKey = new PublicKey();
                publicKey.fromXmlNode(xmlSubNode.SelectSingleNode("ns:publicKey", xmlNamespace), xmlNamespace);
                certificationAuthorityRepository.add(rid, index, publicKey);
            }
        }

        /// <inheritdoc />
        public System.Xml.XmlNode toXmlNode(System.Xml.XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
