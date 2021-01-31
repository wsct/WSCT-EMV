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
            get { return Tlv.Value != null && Tlv.Value.Length >= 5 ? Tlv.Value.ToHexa(5) : String.Empty; }
            set
            {
                if (Tlv.Value == null)
                {
                    Tlv.Value = value.FromHexa();
                }
                else
                {
                    Tlv.Value = value.FromHexa()
                        .Concat(Tlv.Value.Skip(5))
                        .ToArray();
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
                if (Tlv == null || Tlv.Value == null)
                {
                    return String.Empty;
                }

                return Tlv.Value.Skip(5).ToArray().ToHexa();
            }
            set
            {
                if (Tlv.Value == null)
                {
                    Tlv.Value = new byte[] { 0, 0, 0, 0, 0 }.Concat(value.FromHexa()).ToArray();
                }
                else
                {
                    Tlv.Value = Tlv.Value
                        .Take(5)
                        .Concat(value.FromHexa())
                        .ToArray();
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