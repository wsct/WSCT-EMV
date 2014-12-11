using System.Runtime.Serialization;

namespace WSCT.EMV.Security
{
    [DataContract]
    public class PrivateKey
    {
        #region >> Properties

        /// <summary>
        /// Modulus of of Issuer Public Key.
        /// </summary>
        [DataMember]
        public string Modulus { get; set; }

        /// <summary>
        /// Clear issuer private exponent.
        /// Mandatory for SDA only.
        /// </summary>
        [DataMember]
        public string PrivateExponent { get; set; }

        /// <summary>
        /// Exponent of Issuer Public Key.
        /// </summary>
        [DataMember]
        public string PublicExponent { get; set; }

        #endregion

        /// <summary>
        /// Returns a new <see cref="PublicKey"/> using the public exponent.
        /// </summary>
        /// <returns></returns>
        public PublicKey GetPublicKey()
        {
            return new PublicKey(Modulus, PublicExponent);
        }

        /// <summary>
        /// Returns a new <see cref="PublicKey"/> using the private exponent.
        /// </summary>
        /// <returns></returns>
        public PublicKey GetPrivateKey()
        {
            return new PublicKey(Modulus, PrivateExponent);
        }
    }
}
