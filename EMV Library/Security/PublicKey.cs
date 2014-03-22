using System;
using System.Xml.Serialization;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents a public key used for cryptographic purposes
    /// </summary>
    [XmlRoot("publicKey")]
    public class PublicKey
    {
        #region >> Fields

        String _modulus;
        String _exponent;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to the modulus of the public key.
        /// </summary>
        [XmlText]
        public String Modulus
        {
            get { return _modulus; }
            set { _modulus = value.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", ""); }
        }

        /// <summary>
        /// Accessor to the exponent of the public key.
        /// </summary>
        [XmlAttribute("exponent")]
        public String Exponent
        {
            get { return _exponent; }
            set { _exponent = value.Replace(" ", ""); }
        }

        /// <summary>
        /// Accessor to the size of the public key.
        /// </summary>
        [XmlAttribute("size")]
        public String SizeString { get; set; }

        /// <summary>
        /// Accessor to the expiration date of the public key.
        /// </summary>
        [XmlAttribute("expiration")]
        public String DateString { get; set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="PublicKey"/> instance.
        /// </summary>
        public PublicKey()
            : this(String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="PublicKey"/> instance.
        /// </summary>
        /// <param name="modulo">Modulo of the public key.</param>
        /// <param name="exponent">Exponent of the public key.</param>
        public PublicKey(String modulo, String exponent)
        {
            Modulus = modulo;
            Exponent = exponent;
        }

        #endregion
    }
}
