using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Template of an EMV smartcards
    /// </summary>
    public class ApplicationTemplate : AbstractTLVObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationTemplate()
            : base()
        {
        }
    }
}
