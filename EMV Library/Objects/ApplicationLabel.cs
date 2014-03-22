using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Label of an EMV smartcards.
    /// </summary>
    public class ApplicationLabel : StringTLVObject
    {
        /// <summary>
        /// Initializes a new <see cref="ApplicationLabel"/> instance.
        /// </summary>
        public ApplicationLabel()
            : this(String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationLabel"/> instance.
        /// </summary>
        /// <param name="label">Text label of the application.</param>
        public ApplicationLabel(string label)
            : base(0x50, label)
        {
        }
    }
}
