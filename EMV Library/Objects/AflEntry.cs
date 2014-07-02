using System;
using System.Collections.Generic;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents identification informations for one file in the AFL.
    /// </summary>
    public class AflEntry
    {
        #region >> Properties

        /// <summary>
        /// First record to be read.
        /// </summary>
        public byte FirstRecord;

        /// <summary>
        /// Last record to be read.
        /// </summary>
        public byte LastRecord;

        /// <summary>
        /// Last record to be used in offline data authentication (from <c>firstRecord</c> to <c>firstRecord+offlineNumberOfRecords</c>).
        /// </summary>
        public byte OfflineNumberOfRecords;

        /// <summary>
        /// SFI of the file.
        /// </summary>
        public byte Sfi;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="AflEntry"/> instance.
        /// </summary>
        /// <param name="fourBytes">Four bytes identifying the file.</param>
        public AflEntry(byte[] fourBytes)
            : this(fourBytes, 0)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="AflEntry"/> instance.
        /// </summary>
        /// <param name="fourBytes">Four bytes identifying the file.</param>
        /// <param name="offset">Offset in the array <paramref>fourBytes</paramref>.</param>
        public AflEntry(byte[] fourBytes, byte offset)
        {
            Sfi = (byte)(fourBytes[offset + 0] >> 3);
            FirstRecord = fourBytes[offset + 1];
            LastRecord = fourBytes[offset + 2];
            OfflineNumberOfRecords = fourBytes[offset + 3];
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sfi"></param>
        /// <param name="firstRecord"></param>
        /// <param name="lastRecord"></param>
        /// <param name="offlineNumberOfRecords"></param>
        public AflEntry(byte sfi, byte firstRecord, byte lastRecord, byte offlineNumberOfRecords)
        {
            Sfi = sfi;
            FirstRecord = firstRecord;
            LastRecord = lastRecord;
            OfflineNumberOfRecords = offlineNumberOfRecords;
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            return String.Format("sfi:{0} from {1} to {2} (used for offline auth:{3})",
                Sfi, FirstRecord, LastRecord, OfflineNumberOfRecords);
        }

        #endregion

        /// <summary>
        /// Returns a byte[] representation of the instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<byte> ToEnumerable()
        {
            yield return (byte)(Sfi << 3);
            yield return FirstRecord;
            yield return LastRecord;
            yield return OfflineNumberOfRecords;
        }
    }
}