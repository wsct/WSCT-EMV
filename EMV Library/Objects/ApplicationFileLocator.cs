using System;
using System.Collections.Generic;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application File Locator of an EMV application.
    /// </summary>
    public class ApplicationFileLocator : BinaryTlvObject
    {
        #region >> Nested classes

        /// <summary>
        /// Represents identification informations for one file in the AFL.
        /// </summary>
        public class FileIdentification
        {
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

            /// <summary>
            /// Initializes a new <see cref="FileIdentification"/> instance.
            /// </summary>
            /// <param name="fourBytes">Four bytes identifying the file.</param>
            public FileIdentification(byte[] fourBytes)
                : this(fourBytes, 0)
            {
            }

            /// <summary>
            /// Initializes a new <see cref="FileIdentification"/> instance.
            /// </summary>
            /// <param name="fourBytes">Four bytes identifying the file.</param>
            /// <param name="offset">Offset in the array <paramref>fourBytes</paramref>.</param>
            public FileIdentification(byte[] fourBytes, byte offset)
            {
                Sfi = (byte)(fourBytes[offset + 0] >> 3);
                FirstRecord = fourBytes[offset + 1];
                LastRecord = fourBytes[offset + 2];
                OfflineNumberOfRecords = fourBytes[offset + 3];
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return String.Format("sfi:{0} from {1} to {2} (used for offline auth:{3})",
                    Sfi, FirstRecord, LastRecord, OfflineNumberOfRecords);
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="ApplicationFileLocator"/> instance.
        /// </summary>
        public ApplicationFileLocator()
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationFileLocator"/> instance.
        /// </summary>
        /// <param name="aflData">Raw AFL data.</param>
        public ApplicationFileLocator(byte[] aflData)
            : this()
        {
            Tlv = new TlvData { Value = aflData };
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Enumerates files.
        /// </summary>
        /// <returns><see cref="FileIdentification" /> instances.</returns>
        public IEnumerable<FileIdentification> GetFiles()
        {
            if (Tlv != null && Tlv.Value != null)
            {
                byte offset = 0;
                while (offset < Tlv.Value.Length)
                {
                    yield return new FileIdentification(Tlv.Value, offset);
                    offset += 4;
                }
            }
        }

        #endregion
    }
}