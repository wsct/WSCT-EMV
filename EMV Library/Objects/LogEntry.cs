using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Log Entry tag of an EMV application.
    /// </summary>
    public class LogEntry : BinaryTlvObject
    {
        #region >> Properties

        /// <summary>
        /// Accessor to the SFI defined by the log entry.
        /// </summary>
        public byte Sfi
        {
            get { return (byte)(Tlv.Value[0] & 0x1F); }
            set { Tlv.Value[0] = value; }
        }

        /// <summary>
        /// Accessor to the record size defined by the log entry.
        /// </summary>
        public byte CyclicFileSize
        {
            get { return Tlv.Value[1]; }
            set { Tlv.Value[1] = value; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogEntry()
            : this(new TlvData(0x9F4D, 2, new byte[] { 0x00, 0x00 }))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tlvLogEntry">Raw Log Entry tag</param>
        public LogEntry(TlvData tlvLogEntry)
        {
            if (tlvLogEntry.Length < 2)
            {
                throw new Exception(String.Format("LogEntry: TLV data length < 2, can't parse [{0}]", tlvLogEntry));
            }
            Tlv = tlvLogEntry;
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