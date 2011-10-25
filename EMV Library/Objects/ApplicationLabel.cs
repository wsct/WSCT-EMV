using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Label of an EMV smartcards
    /// </summary>
    public class ApplicationLabel : StringTLVObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationLabel()
            : base()
        {
        }
    }
}
