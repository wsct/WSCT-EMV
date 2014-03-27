﻿using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Cryptogram generate by an EMV card.
    /// </summary>
    public class ApplicationCryptogram : BinaryTlvObject
    {
        #region >> Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationCryptogram()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationCryptogram"/> instance.
        /// </summary>
        /// <param name="tlvAC">TLVData containing AID.</param>
        public ApplicationCryptogram(TlvData tlvAC)
            : this()
        {
            Tlv = tlvAC;
        }

        #endregion
    }
}