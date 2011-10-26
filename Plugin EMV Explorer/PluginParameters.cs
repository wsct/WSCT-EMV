using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    public class PluginParameters : WSCT.Helpers.Xml.IXmlSerializable
    {
        #region >> Fields

        TerminalParameters _terminalParameters;
        TransactionParameters _transactionParameters;

        #endregion

        #region >> Properties

        public TerminalParameters terminalParameters
        {
            get { return _terminalParameters; }
        }

        public TransactionParameters transactionParameters
        {
            get { return _transactionParameters; }
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Loads a set of descriptors from an XML file
        /// </summary>
        /// <param name="xmlFileName">Name of the file to read.</param>
        /// <returns><c>true</c> if successful load, or an exception will be raised.</returns>
        public Boolean loadFromXml(String fileName)
        {
            try
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(fileName))
                {
                    xmlDoc.Load(xmlReader);
                    xmlReader.Close();
                }
                System.Xml.XmlNamespaceManager namespaceManager = new System.Xml.XmlNamespaceManager(xmlDoc.NameTable);
                namespaceManager.AddNamespace("ns", xmlDoc.DocumentElement.NamespaceURI);
                System.Xml.XmlNodeList cardTagNodeList = xmlDoc.SelectNodes("//ns:pluginEMVExplorer", namespaceManager);

                foreach (System.Xml.XmlNode cardTagNode in cardTagNodeList)
                {
                    fromXmlNode(cardTagNode, namespaceManager);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error(s) found when parsing XML file '" + fileName + "'\n" + e.Message);
            }
            return true;
        }

        #endregion

        #region >> IXmlSerializable Members

        /// <inheritdoc />
        public void fromXmlNode(XmlNode xmlNode, XmlNamespaceManager xmlNamespace)
        {
            foreach (System.Xml.XmlNode subNode in xmlNode)
            {
                switch (subNode.Name)
                {
                    case "terminal":
                        _terminalParameters = new TerminalParameters();
                        _terminalParameters.fromXmlNode(subNode, xmlNamespace);
                        break;
                    case "transaction":
                        _transactionParameters = new TransactionParameters();
                        _transactionParameters.fromXmlNode(subNode, xmlNamespace);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <inheritdoc />
        public XmlNode toXmlNode(XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
