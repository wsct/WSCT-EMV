using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Certification Authority Public Key Index of an EMV Application.
    /// </summary>
    public class CertificationAuthorityPublicKeyIndex : BinaryTLVObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the index.
        /// </summary>
        public Byte Index
        {
            get { return tlv.value[0]; }
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
        public CertificationAuthorityPublicKeyIndex(TLVData tlvCaPublicKeyIndex)
        {
            tlv = tlvCaPublicKeyIndex;
        }

        #endregion
    }
}
