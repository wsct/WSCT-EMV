using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;
using WSCT.EMV.Security;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    public class TransactionParameters : WSCT.Helpers.Xml.IXmlSerializable
    {
        #region >> Fields

        List<TLVData> _tlvDatas;

        #endregion

        #region >> Properties

        public List<TLVData> tlvDatas
        {
            get { return _tlvDatas; }
            set { _tlvDatas = value; }
        }

        #endregion

        #region >> Constructors

        public TransactionParameters()
        {
            _tlvDatas = new List<TLVData>();
        }

        #endregion

        #region >> IXmlSerializable Members

        /// <inheritdoc />
        public void fromXmlNode(System.Xml.XmlNode xmlNode, System.Xml.XmlNamespaceManager xmlNamespace)
        {
            // Loads default terminal datas
            tlvDatas = new List<TLVData>();

            foreach (System.Xml.XmlNode xmlSubNode in xmlNode.SelectNodes("ns:tagValues/ns:tagValue", xmlNamespace))
            {
                TLVData tlv = new TLVData();
                foreach (System.Xml.XmlAttribute xmlAttr in xmlSubNode.Attributes)
                {
                    switch (xmlAttr.Name)
                    {
                        case "tag":
                            tlv.parseT(xmlAttr.Value.fromHexa(), (uint)0);
                            break;
                        default:
                            break;
                    }
                }
                tlv.length = (uint)(xmlSubNode.InnerText.Length / 2);
                tlv.parseV(xmlSubNode.InnerText.fromHexa(), 0);
                tlvDatas.Add(tlv);
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
