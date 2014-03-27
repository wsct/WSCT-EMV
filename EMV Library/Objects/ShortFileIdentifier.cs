using System.Globalization;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Short File Identifier of an EMV smartcards.
    /// </summary>
    public class ShortFileIdentifier : AbstractTlvObject
    {
        #region >> Properties

        /// <summary>
        /// SFI value defined by the tag.
        /// </summary>
        public byte Sfi
        {
            get { return Tlv.Value[0]; }
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
        public ShortFileIdentifier(TlvData tlvSfi)
        {
            Tlv = tlvSfi;
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