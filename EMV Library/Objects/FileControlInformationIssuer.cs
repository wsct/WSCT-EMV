using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the File Control Information Issuer used in EMV smartcards
    /// </summary>
    public class FileControlInformationIssuer : AbstractTLVObject
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileControlInformationIssuer()
            : base()
        {
        }
    }
}
