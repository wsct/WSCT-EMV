using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Tags
{
    /// <summary>
    /// Represents the TLV formatted as YYMMDD date format.
    /// </summary>
    public class DateFormattedTag : AbstractTlvObject
    {
        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            return String.Format("{4}{5}/{2}{3}/20{0}{1}",
                Tlv.Value[0]/16, Tlv.Value[0]%16,
                Tlv.Value[1]/16, Tlv.Value[1]%16,
                Tlv.Value[2]/16, Tlv.Value[2]%16);
        }

        #endregion
    }
}