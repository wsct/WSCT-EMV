using System.Xml.Serialization;

using WSCT.EMV.Security;

namespace WSCT.EMV.Terminal
{
    [XmlRoot("terminalConfiguration")]
    public class TerminalConfiguration
    {
        #region >> Fields

        TerminalCapabilities _terminalCapabilities;
        CertificationAuthorityRepository _certificationAuthorityRepository;

        #endregion

        #region >> Properties

        [XmlElement("capabilities")]
        public TerminalCapabilities terminalCapabilities
        {
            get { return _terminalCapabilities; }
            set { _terminalCapabilities = value; }
        }

        [XmlElement("certificationAuthorities")]
        public CertificationAuthorityRepository certificationAuthorityRepository
        {
            get { return _certificationAuthorityRepository; }
            set { _certificationAuthorityRepository = value; }
        }

        #endregion

        #region >> Constructors

        public TerminalConfiguration()
        {
            _terminalCapabilities = new TerminalCapabilities();
            _certificationAuthorityRepository = new CertificationAuthorityRepository();
        }

        #endregion
    }
}
