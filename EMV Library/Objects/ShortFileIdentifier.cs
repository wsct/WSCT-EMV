using System;
using System.Globalization;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Short File Identifier of an EMV smartcards.
    /// </summary>
    public class ShortFileIdentifier : AbstractTLVObject
    {
        #region >> Properties

        /// <summary>
        /// SFI value defined by the tag.
        /// </summary>
        public Byte Sfi
        {
            get { return tlv.value[0]; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="ShortFileIdentifier"/> instance.
        /// </summary>
        public ShortFileIdentifier()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ShortFileIdentifier"/> instance.
        /// </summary>
        /// <param name="tlvSfi">TLVData defining the SFI tag</param>
        public ShortFileIdentifier(TLVData tlvSfi)
        {
            tlv = tlvSfi;
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            return Sfi.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
