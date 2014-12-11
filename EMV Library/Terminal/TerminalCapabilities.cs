using System.Collections.Generic;
using System.Xml.Serialization;

namespace WSCT.EMV.Terminal
{
    /// <summary>
    /// Terminal capabilities container.
    /// </summary>
    [XmlRoot("capabilities")]
    public class TerminalCapabilities
    {
        #region >> Nested classes

        /// <summary>
        /// Represents an application supported by the terminal.
        /// </summary>
        [XmlRoot("application")]
        public class SupportedApplication
        {
            /// <summary>
            /// AID of the application (text).
            /// </summary>
            [XmlAttribute("aid")]
            public string Aid { get; set; }

            /// <summary>
            /// Label of the application.
            /// </summary>
            [XmlAttribute("label")]
            public string Label { get; set; }
        }

        /// <summary>
        /// Represents a PSE supported by the terminal.
        /// </summary>
        [XmlRoot("pse")]
        public class SupportedPSE
        {
            /// <summary>
            /// DF Name of the PSE.
            /// </summary>
            [XmlAttribute("name")]
            public string Name { get; set; }

            /// <summary>
            /// Label of the PSE.
            /// </summary>
            [XmlAttribute("label")]
            public string Label { get; set; }
        }

        #endregion

        #region >> Properties

        /// <summary>
        /// List of supported applications.
        /// </summary>
        [XmlArray("supportedApplications")]
        [XmlArrayItem("application")]
        public List<SupportedApplication> SupportedApplications { get; set; }

        /// <summary>
        /// List of supporterd PSE.
        /// </summary>
        [XmlArray("supportedPSEs")]
        [XmlArrayItem("pse")]
        public List<SupportedPSE> SupportedPses { get; set; }

        #endregion
    }
}