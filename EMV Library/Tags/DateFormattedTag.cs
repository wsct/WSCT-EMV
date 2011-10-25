using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV.Tags;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Tags
{

    /// <summary>
    /// Represents the TLV formatted as YYMMDD date format
    /// </summary>
    public class DateFormattedTag : AbstractTLVObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DateFormattedTag()
            : base()
        {
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return String.Format("{4}{5}/{2}{3}/20{0}{1}",
                tlv.value[0] / 16, tlv.value[0] % 16,
                tlv.value[1] / 16, tlv.value[1] % 16,
                tlv.value[2] / 16, tlv.value[2] % 16);
        }
    }
}
