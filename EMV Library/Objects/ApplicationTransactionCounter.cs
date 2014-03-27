using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Transaction Counter of an EMV card.
    /// </summary>
    public class ApplicationTransactionCounter : BinaryTlvObject
    {
        #region >> Properties

        /// <summary>
        /// ATC value.
        /// </summary>
        public UInt16 Counter
        {
            get { return (UInt16)(Tlv.Value[0]*0x100 + Tlv.Value[1]); }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="ApplicationTransactionCounter"/> instance.
        /// </summary>
        public ApplicationTransactionCounter()
        {
            Tlv = new TlvData(0x9F36, 02, new byte[2]);
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationTransactionCounter"/> instance.
        /// </summary>
        /// <param name="tlvATC">TLV TVR data.</param>
        public ApplicationTransactionCounter(TlvData tlvATC)
            : this()
        {
            Tlv = tlvATC;
        }

        #endregion
    }
}