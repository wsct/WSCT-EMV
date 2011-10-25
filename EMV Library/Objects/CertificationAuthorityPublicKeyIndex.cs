using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Certification Authority Public Key Index of an EMV Application
    /// </summary>
    public class CertificationAuthorityPublicKeyIndex : BinaryTLVObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the index
        /// </summary>
        public Byte index
        {
            get { return tlv.value[0]; }
        }

        #endregion

        #region >> Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CertificationAuthorityPublicKeyIndex()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvCAPublicKeyIndex">TLVData defining the Certification Authority Public Key Index tag</param>
        public CertificationAuthorityPublicKeyIndex(TLVData tlvCAPublicKeyIndex)
        {
            tlv = tlvCAPublicKeyIndex;
        }

        #endregion
    }
}
