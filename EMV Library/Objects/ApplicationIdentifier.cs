using System;
using System.Linq;
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
        public string Rid
        {
            get { return Tlv.Value is not null && Tlv.Value.Length >= 5 ? Tlv.Value.ToHexa(5) : String.Empty; }
            set
            {
                if (Tlv.Value is null)
                {
                    Tlv.Value = value.FromHexa();
                }
                else
                {
                    Tlv.Value = [.. value.FromHexa(), .. Tlv.Value.Skip(5)];
                }
            }
        }

        /// <summary>
        /// Accessor to PIX part of the AID.
        /// </summary>
        public string Pix
        {
            get
            {
                if (Tlv is null || Tlv.Value is null)
                {
                    return String.Empty;
                }

                return Tlv.Value[5..].ToHexa();
            }
            set
            {
                if (Tlv.Value is null)
                {
                    Tlv.Value = [0, 0, 0, 0, 0, .. value.FromHexa()];
                }
                else
                {
                    Tlv.Value = [.. Tlv.Value[..5], .. value.FromHexa()];
                }
            }
        }

        #endregion

        #region >> Constructor

        /// <summary>
        /// Initializes a new <see cref="ApplicationIdentifier"/> instance.
        /// </summary>
        public ApplicationIdentifier()
        {
            Tlv = new TlvData { Tag = 0x4F };
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationIdentifier"/> instance.
        /// Constructor
        /// </summary>
        /// <param name="tlvAid">TLVData containing AID.</param>
        public ApplicationIdentifier(TlvData tlvAid)
        {
            Tlv = tlvAid;
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationIdentifier"/> instance.
        /// </summary>
        /// <param name="sAid">string AID.</param>
        public ApplicationIdentifier(string sAid)
        {
            var value = sAid.FromHexa();
            Tlv = new TlvData(0x4F, (UInt32)value.Length, value);
        }

        #endregion
    }
}