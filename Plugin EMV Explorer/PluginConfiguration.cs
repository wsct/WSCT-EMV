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
    }
}