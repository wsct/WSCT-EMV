using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Language Preference used in EMV smartcards.
    /// </summary>
    public class LanguagePreference : StringTLVObject
    {
        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="LanguagePreference"/> instance.
        /// </summary>
        public LanguagePreference()
            : this(String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="LanguagePreference"/> instance.
        /// </summary>
        /// <param name="langs">Prefered languages of the application.</param>
        public LanguagePreference(string langs)
            : base(0x5F2D, langs)
        {
        }

        #endregion
    }
}
