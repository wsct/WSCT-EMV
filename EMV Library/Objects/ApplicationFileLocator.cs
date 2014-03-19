using System;
using System.Collections;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application File Locator of an EMV application.
    /// </summary>
    public class ApplicationFileLocator : BinaryTLVObject
    {
        #region >> Nested classes

        /// <summary>
        /// Represents identification informations for one file in the AFL.
        /// </summary>
        public class FileIdentification
        {
            /// <summary>
            /// SFI of the file.
            /// </summary>
            public Byte Sfi;

            /// <summary>
            /// First record to be read.
            /// </summary>
            public Byte FirstRecord;

            /// <summary>
            /// Last record to be read.
            /// </summary>
            public Byte LastRecord;

            /// <summary>
            /// Last record to be used in offline data authentication (from <c>firstRecord</c> to <c>firstRecord+offlineNumberOfRecords</c>).
            /// </summary>
            public Byte OfflineNumberOfRecords;

            /// <summary>
            /// Initializes a new <see cref="FileIdentification"/> instance.
            /// </summary>
            /// <param name="fourBytes">Four bytes identifying the file.</param>
            public FileIdentification(Byte[] fourBytes)
                : this(fourBytes, 0)
            {
            }

            /// <summary>
            /// Initializes a new <see cref="FileIdentification"/> instance.
            /// </summary>
            /// <param name="fourBytes">Four bytes identifying the file.</param>
            /// <param name="offset">Offset in the array <paramref>fourBytes</paramref>.</param>
            public FileIdentification(Byte[] fourBytes, Byte offset)
            {
                Sfi = (Byte)(fourBytes[offset + 0] >> 3);
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
        public ApplicationFileLocator(Byte[] aflData)
            : this()
        {
            tlv = new TLVData { value = aflData };
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Enumerates files.
        /// </summary>
        /// <returns><see cref="FileIdentification" /> instances.</returns>
        public IEnumerable GetFiles()
        {
            Byte offset = 0;
            while (offset < tlv.value.Length)
            {
                yield return new FileIdentification(tlv.value, offset);
                offset += 4;
            }
        }

        #endregion
    }
}
