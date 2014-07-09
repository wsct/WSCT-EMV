using System;
using System.Collections.Generic;
using System.Linq;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Language Preference used in EMV smartcards.
    /// </summary>
    public class LanguagePreference : StringTlvObject
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

        /// <summary>
        /// Initializes a new <see cref="LanguagePreference"/> instance.
        /// </summary>
        /// <param name="langs">Prefered languages of the application.</param>
        public LanguagePreference(IEnumerable<string> langs)
            : base(0x5F2D, langs.Aggregate(String.Empty, (l, c) => c + l))
        {
        }

        #endregion

        public IEnumerable<string> Languages
        {
            get
            {
                var length = Tlv.Value.Length;
                for (var i = 0; i + 1 < length; i += 2)
                {
                    yield return new[] { Tlv.Value[i], Tlv.Value[i + 1] }.ToAsciiString();
                }

                if (length % 2 == 1)
                {
                    yield return new[] { Tlv.Value[length - 1] }.ToAsciiString();
                }
            }
            set
            {
                Text = string.Empty;
                foreach (var s in value)
                {
                    Text += s;
                }
            }
        }
    }
}