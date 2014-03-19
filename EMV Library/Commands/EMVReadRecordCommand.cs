using System;
using WSCT.ISO7816.Commands;

namespace WSCT.EMV.Commands
{
    /// <summary>
    /// EMV Read Record C-APDU.
    /// </summary>
    public class EMVReadRecordCommand : ReadRecordCommand
    {
        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="EMVReadRecordCommand"/> instance.
        /// </summary>
        public EMVReadRecordCommand()
        {
            searchMode = SearchMode.READ_RECORD_P1;
        }

        /// <summary>
        /// Initializes a new <see cref="EMVReadRecordCommand"/> instance.
        /// </summary>
        /// <param name="record">Record number to read.</param>
        /// <param name="sfi">Short File Identifier of the EF to read.</param>
        /// <param name="le">Length of data to read.</param>
        public EMVReadRecordCommand(Byte record, Byte sfi, UInt32 le)
            : base(record, sfi, SearchMode.READ_RECORD_P1, le)
        {
        }

        #endregion
    }
}
