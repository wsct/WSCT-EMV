using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Transaction Counter of an EMV card
    /// </summary>
    public class ApplicationTransactionCounter : BinaryTLVObject
    {
        #region >> Properties

        /// <summary>
        /// ATC value
        /// </summary>
        public UInt16 counter
        {
            get { return (UInt16)(tlv.value[0] * 0x100 + tlv.value[1]); }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationTransactionCounter()
            : base()
        {
            tlv = new TLVData();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvATC">TLV TVR data</param>
        public ApplicationTransactionCounter(TLVData tlvATC)
            : this()
        {
            tlv = tlvATC;
        }

        #endregion
    }
}
