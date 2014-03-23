using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Log Entry tag of an EMV application.
    /// </summary>
    public class LogEntry : BinaryTLVObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the SFI defined by the log entry.
        /// </summary>
        public byte Sfi
        {
            get { return (byte)(tlv.value[0] & 0x8F); }
            set { tlv.value[0] = value; }
        }

        /// <summary>
        /// Accessor to the record size defined by the log entry.
        /// </summary>
        public byte CyclicFileSize
        {
            get { return tlv.value[1]; }
            set { tlv.value[1] = value; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogEntry()
            : this(new TLVData(0x9F4D, 2, new byte[] { 0x00, 0x00 }))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvLogEntry">Raw Log Entry tag</param>
        public LogEntry(TLVData tlvLogEntry)
        {
            if (tlvLogEntry.length < 2)
            {
                throw new Exception(String.Format("LogEntry: TLV data length < 2, can't parse [{0}]", tlvLogEntry));
            }
            tlv = tlvLogEntry;
        }

        #endregion

        #region >> Onject

        /// <inheritdoc />
        public override string ToString()
        {
            return String.Format("sfi:{0:X} record size:{1:X}", Sfi, CyclicFileSize);
        }

        #endregion
    }
}
