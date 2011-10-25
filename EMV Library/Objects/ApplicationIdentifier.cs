using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Identifier (AID) of an EMV smartcards
    /// </summary>
    public class ApplicationIdentifier : BinaryTLVObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the RID part of the AID
        /// </summary>
        public String strRID
        {
            get { return tlv.value.toHexa(5); }
        }

        /// <summary>
        /// Accessor to PIX part of the AID
        /// </summary>
        public String strPIX
        {
            get
            {
                Byte[] pix = new Byte[tlv.value.Length - 5];
                Array.Copy(tlv.value, 5, pix, 0, pix.Length);
                return pix.toHexa();
            }
        }

        #endregion

        #region >> Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationIdentifier()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvAID">TLVData containing AID</param>
        public ApplicationIdentifier(TLVData tlvAID)
            : this()
        {
            tlv = tlvAID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sAID">String AID</param>
        public ApplicationIdentifier(String sAID)
        {
            tlv = new TLVData(0x4F, (UInt32)sAID.Length, sAID.fromHexa());
        }

        #endregion
    }
}
