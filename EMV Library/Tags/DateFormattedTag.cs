using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Tags
{

    /// <summary>
    /// Represents the TLV formatted as YYMMDD date format.
    /// </summary>
    public class DateFormattedTag : AbstractTLVObject
    {
        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            return String.Format("{4}{5}/{2}{3}/20{0}{1}",
                tlv.value[0] / 16, tlv.value[0] % 16,
                tlv.value[1] / 16, tlv.value[1] % 16,
                tlv.value[2] / 16, tlv.value[2] % 16);
        }

        #endregion
    }
}
