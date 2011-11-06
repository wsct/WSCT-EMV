using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WSCT.EMV.Terminal
{
    [XmlRoot("capabilities")]
    public class TerminalCapabilities
    {
        #region >> Nested classes

        [XmlRoot("application")]
        public class SupportedApplication
        {
            [XmlAttribute("aid")]
            public String aid
            { get; set; }

            [XmlAttribute("label")]
            public String label
            { get; set; }
        }

        [XmlRoot("pse")]
        public class SupportedPSE
        {
            [XmlAttribute("name")]
            public String name
            { get; set; }

            [XmlAttribute("label")]
            public String label
            { get; set; }
        }

        #endregion

        #region >> Properties

        [XmlArray("supportedApplications")]
        [XmlArrayItem("application")]
        public List<SupportedApplication> supportedApplications
        { get; set; }

        [XmlArray("supportedPSEs")]
        [XmlArrayItem("pse")]
        public List<SupportedPSE> supportedPSEs
        { get; set; }

        #endregion
    }
}
