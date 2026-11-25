using System.Xml.Serialization;
using WSCT.EMV.Terminal;
using WSCT.EMV.Transaction;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    [XmlRoot("pluginConfiguration")]
    public class PluginConfiguration
    {
        [XmlElement("terminalConfiguration")]
        public TerminalConfiguration terminalConfiguration { get; set; } = new();

        [XmlElement("transactionContext")]
        public TransactionContext transactionContext { get; set; } = new();
    }
}
