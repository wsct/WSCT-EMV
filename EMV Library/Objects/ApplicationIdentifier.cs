using System;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Identifier (AID) of an EMV smartcards.
    /// </summary>
    public class ApplicationIdentifier : BinaryTLVObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the RID part of the AID.
        /// </summary>
        public String StrRid
        {
            get
            {
                return (tlv != null && tlv.value != null ? tlv.value.toHexa(5) : String.Empty);
            }
        }

        /// <summary>
        /// Accessor to PIX part of the AID.
        /// </summary>
        public String StrPix
        {
            get
            {
                if (tlv != null && tlv.value != null)
                {
                    var pix = new Byte[tlv.value.Length - 5];
                    Array.Copy(tlv.value, 5, pix, 0, pix.Length);
                    return pix.toHexa();
                }

                return String.Empty;
            }
        }

        #endregion

        #region >> Constructor

        /// <summary>
        /// Initializes a new <see cref="ApplicationIdentifier"/> instance.
        /// </summary>
        public ApplicationIdentifier()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationIdentifier"/> instance.
        /// Constructor
        /// </summary>
        /// <param name="tlvAid">TLVData containing AID.</param>
        public ApplicationIdentifier(TLVData tlvAid)
            : this()
        {
            tlv = tlvAid;
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationIdentifier"/> instance.
        /// </summary>
        /// <param name="sAid">String AID.</param>
        public ApplicationIdentifier(String sAid)
        {
            var value = sAid.fromHexa();
            tlv = new TLVData(0x4F, (UInt32)value.Length, value);
        }

        #endregion
    }
}
