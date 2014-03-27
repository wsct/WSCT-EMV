using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Certification Authority Public Key Index of an EMV Application.
    /// </summary>
    public class CertificationAuthorityPublicKeyIndex : BinaryTlvObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the index.
        /// </summary>
        public byte Index
        {
            get { return Tlv.Value[0]; }
        }

        #endregion

        #region >> Constructor

        /// <summary>
        /// Initializes a new <see cref="CertificationAuthorityPublicKeyIndex"/> instance.
        /// </summary>
        public CertificationAuthorityPublicKeyIndex()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="CertificationAuthorityPublicKeyIndex"/> instance.
        /// </summary>
        /// <param name="tlvCaPublicKeyIndex">TLVData defining the Certification Authority Public Key Index tag.</param>
        public CertificationAuthorityPublicKeyIndex(TlvData tlvCaPublicKeyIndex)
        {
            Tlv = tlvCaPublicKeyIndex;
        }

        #endregion
    }
}