using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the File Control Information Proprietary of an EMV smartcards
    /// </summary>
    public class FileControlInformationProprietary : AbstractTLVObject
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileControlInformationProprietary()
            : base()
        {
        }
    }
}
