﻿using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Cryptogram generate by an EMV card
    /// </summary>
    public class ApplicationCryptogram : BinaryTLVObject
    {
        #region >> Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationCryptogram()
            : base()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvAC">TLVData containing AID</param>
        public ApplicationCryptogram(TLVData tlvAC)
            : this()
        {
            tlv = tlvAC;
        }

        #endregion
    }
}
