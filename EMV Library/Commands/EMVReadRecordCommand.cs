using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSCT.ISO7816;
using WSCT.ISO7816.Commands;

namespace WSCT.EMV.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class EMVReadRecordCommand : ReadRecordCommand
    {
        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EMVReadRecordCommand()
            : base()
        {
            this.searchMode = SearchMode.READ_RECORD_P1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="sfi"></param>
        /// <param name="le"></param>
        public EMVReadRecordCommand(Byte record, Byte sfi, UInt32 le)
            : base(record, sfi, SearchMode.READ_RECORD_P1, le)
        {
        }

        #endregion
    }
}
