using System;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Identifier (AID) of an EMV smartcards.
    /// </summary>
    public class ApplicationIdentifier : BinaryTlvObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the RID part of the AID.
        /// </summary>
        public String StrRid
        {
            get { return (Tlv != null && Tlv.Value != null ? Tlv.Value.ToHexa(5) : String.Empty); }
        }

        /// <summary>
        /// Accessor to PIX part of the AID.
        /// </summary>
        public String StrPix
        {
            get
            {
                if (Tlv != null && Tlv.Value != null)
                {
                    var pix = new byte[Tlv.Value.Length - 5];
                    Array.Copy(Tlv.Value, 5, pix, 0, pix.Length);
                    return pix.ToHexa();
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
        public ApplicationIdentifier(TlvData tlvAid)
            : this()
        {
            Tlv = tlvAid;
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationIdentifier"/> instance.
        /// </summary>
        /// <param name="sAid">String AID.</param>
        public ApplicationIdentifier(String sAid)
        {
            var value = sAid.FromHexa();
            Tlv = new TlvData(0x4F, (UInt32)value.Length, value);
        }

        #endregion
    }
}