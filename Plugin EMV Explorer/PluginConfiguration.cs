using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using WSCT.EMV.Terminal;
using WSCT.EMV.Transaction;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    [XmlRoot("pluginConfiguration")]
    public class PluginConfiguration
    {
        #region >> Fields

        private TerminalConfiguration _terminalConfiguration;
        private TransactionContext _transactionContext;

        #endregion

        #region >> Properties

        [XmlElement("terminalConfiguration")]
        public TerminalConfiguration terminalConfiguration
        {
            get { return _terminalConfiguration; }
            set { _terminalConfiguration = value; }
        }

        [XmlElement("transactionContext")]
        public TransactionContext transactionContext
        {
            get { return _transactionContext; }
            set { _transactionContext = value; }
        }

        #endregion

        #region >> Static methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFileName"></param>
        /// <returns></returns>
        public static PluginConfiguration loadFromXml(String xmlFileName)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(PluginConfiguration));
            PluginConfiguration pluginConfiguration;

            using (TextReader textReader = new StreamReader(xmlFileName))
            {
                pluginConfiguration = (PluginConfiguration)deserializer.Deserialize(textReader);
            }
            return pluginConfiguration;
        }

        #endregion
    }
}
