using System.Xml.Serialization;

using WSCT.EMV.Security;

namespace WSCT.EMV.Terminal
{
    /// <summary>
    /// Configuration of the terminal.
    /// </summary>
    [XmlRoot("terminalConfiguration")]
    public class TerminalConfiguration
    {
        #region >> Properties

        /// <summary>
        /// Terminal capabilities.
        /// </summary>
        [XmlElement("capabilities")]
        public TerminalCapabilities TerminalCapabilities { get; set; }

        /// <summary>
        /// Repository of AC.
        /// </summary>
        [XmlElement("certificationAuthorities")]
        public CertificationAuthorityRepository CertificationAuthorityRepository { get; set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="TerminalConfiguration"/> instance.
        /// </summary>
        public TerminalConfiguration()
        {
            TerminalCapabilities = new TerminalCapabilities();
            CertificationAuthorityRepository = new CertificationAuthorityRepository();
        }

        #endregion
    }
}
