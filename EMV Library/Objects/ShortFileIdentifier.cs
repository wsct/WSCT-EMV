using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Short File Identifier of an EMV smartcards
    /// </summary>
    public class ShortFileIdentifier : AbstractTLVObject
    {
        #region >> Properties

        /// <summary>
        /// SFI value defined by the tag
        /// </summary>
        public Byte sfi
        {
            get { return tlv.value[0]; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ShortFileIdentifier()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvSFI">TLVData defining the SFI tag</param>
        public ShortFileIdentifier(TLVData tlvSFI)
        {
            tlv = tlvSFI;
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return sfi.ToString(); ;
        }
    }
}
