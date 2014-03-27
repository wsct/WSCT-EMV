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
            Search = SearchMode.ReadRecordP1;
        }

        /// <summary>
        /// Initializes a new <see cref="EMVReadRecordCommand"/> instance.
        /// </summary>
        /// <param name="record">Record number to read.</param>
        /// <param name="sfi">Short File Identifier of the EF to read.</param>
        /// <param name="le">Length of data to read.</param>
        public EMVReadRecordCommand(byte record, byte sfi, UInt32 le)
            : base(record, sfi, SearchMode.ReadRecordP1, le)
        {
        }

        #endregion
    }
}