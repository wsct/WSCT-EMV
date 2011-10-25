using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents a public key used for cryptographic purposes
    /// </summary>
    public class PublicKey : WSCT.Helpers.Xml.IXmlSerializable
    {
        #region >> Fields

        String _modulus;
        String _exponent;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to the modulus of the public key
        /// </summary>
        public String modulus
        {
            get { return _modulus; }
            set { _modulus = value.Replace(" ", ""); }
        }

        /// <summary>
        /// Accessor to the exponent of the public key
        /// </summary>
        public String exponent
        {
            get { return _exponent; }
            set { _exponent = value.Replace(" ", ""); }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PublicKey()
            : this(String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modulo">Modulo of the public key</param>
        /// <param name="exponent">Exponent of the public key</param>
        public PublicKey(String modulo, String exponent)
        {
            this.modulus = modulo;
            this.exponent = exponent;
        }

        #endregion

        #region >> IXmlSerializable Members

        /// <inheritdoc />
        public void fromXmlNode(System.Xml.XmlNode xmlNode, System.Xml.XmlNamespaceManager xmlNamespace)
        {
            foreach (System.Xml.XmlAttribute xmlAttr in xmlNode.Attributes)
            {
                switch (xmlAttr.Name)
                {
                    case "exponent":
                        exponent = xmlAttr.Value;
                        break;
                    case "size":
                        // TODO: something with it
                        break;
                    case "expiration":
                        // TODO: something with it
                        break;
                    default:
                        break;
                }
            }

            modulus = xmlNode.InnerXml.Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }

        /// <inheritdoc />
        public System.Xml.XmlNode toXmlNode(System.Xml.XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
