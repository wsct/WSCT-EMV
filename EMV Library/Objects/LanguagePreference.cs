using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Language Preference used in EMV smartcards
    /// </summary>
    public class LanguagePreference : StringTLVObject
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LanguagePreference()
            : base()
        {
        }
    }
}
