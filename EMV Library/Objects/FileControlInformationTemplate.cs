using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the File Control Information Template used in EMV smartcards
    /// </summary>
    public class FileControlInformationTemplate : AbstractTLVObject
    {
        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileControlInformationTemplate()
            : base()
        {
        }

        #endregion
    }
}
